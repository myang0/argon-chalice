using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BaseChamber : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected List<Button> buttons = new List<Button>();
    [SerializeField] protected List<GateArea> gates = new List<GateArea>();
    [SerializeField] protected List<Transform> nextSpawnPoint = new List<Transform>();
    [SerializeField] protected Tilemap gateTileMap;
    [SerializeField] protected ChamberBoundary chamberBoundary;
    [SerializeField] protected BoxCollider cameraBoundary;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (Time.timeScale != 0) {
            PlayerEnterGate();
            if (Input.GetKeyDown(KeyCode.O) && chamberBoundary.GetChamberIsActive()) {
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(gameObject.scene.name, LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync(currentScene);
                GameObject.FindWithTag("PlayerCharacter").GetComponent<CharacterBehavior>().ResetSpawn();
            }
        }
    }

    private void FixedUpdate() {
        SetCamera();
        SetGate();
    }

    protected virtual void SetCamera() {
        if (chamberBoundary.GetChamberIsActive()) {
            GameObject.FindGameObjectWithTag("Camera")
                .GetComponent<CinemachineConfiner>()
                .m_BoundingVolume = cameraBoundary;
        }
    }

    protected virtual void PlayerEnterGate() {
        foreach (GateArea gate in gates) {
            if (gate.GetIsPlayerNearby() && Input.GetKeyDown(KeyCode.E) && GetChamberComplete()) {
                ProceedChamber();
            }
        }
    }

    public virtual bool GetChamberComplete() {
        foreach (Button button in buttons) {
            if (!button.GetIsPushed()) {
                return false;
            }
        }
        return true;
    }

    protected virtual void ProceedChamber() {
        GameObject player = GameObject.FindWithTag("PlayerCharacter");
        player.transform.position = nextSpawnPoint[0].position;
        player.GetComponent<CharacterBehavior>().spawnPoint = nextSpawnPoint[0].position;
    }

    protected virtual void SetGate() {
        gateTileMap.gameObject.SetActive(!GetChamberComplete());
    }
}

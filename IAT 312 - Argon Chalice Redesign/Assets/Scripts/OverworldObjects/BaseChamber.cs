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
    [SerializeField] protected List<BasicButton> buttons = new List<BasicButton>();
    [SerializeField] protected List<GateArea> gates = new List<GateArea>();
    // [SerializeField] protected List<Transform> nextSpawnPoint = new List<Transform>();
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
                foreach (Transform child in transform) {
                    ResettableObject resettable = child.gameObject.GetComponent<ResettableObject>();
                    if (resettable) {
                        resettable.ResetObject();
                    }
                }
                GameObject.FindWithTag("PlayerCharacter").GetComponent<CharacterBehavior>().ResetObject();
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
        if (buttons.Count == 0) return true;
        foreach (BasicButton button in buttons) {
            if (!button.GetIsPushed()) {
                return false;
            }
        }
        return true;
    }

    protected virtual void ProceedChamber() {
        GameObject player = GameObject.FindWithTag("PlayerCharacter");
        Transform nextSpawnPoint = player.GetComponent<OverWorldManager>().GetNextSpawn();
        player.transform.position = nextSpawnPoint.position;
        player.GetComponent<CharacterBehavior>().spawnPoint = nextSpawnPoint.position;
    }

    protected virtual void SetGate() {
        gateTileMap.gameObject.SetActive(!GetChamberComplete());
    }
}

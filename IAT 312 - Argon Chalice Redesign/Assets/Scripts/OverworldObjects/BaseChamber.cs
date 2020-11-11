using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BaseChamber : MonoBehaviour
{
    [SerializeField] protected List<BasicButton> buttons = new List<BasicButton>();
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip openSound;
    [SerializeField] protected AudioClip closeSound;
    [SerializeField] protected List<GateArea> gates = new List<GateArea>();
    [SerializeField] protected Tilemap gateTileMap;
    [SerializeField] protected ChamberBoundary chamberBoundary;
    [SerializeField] protected BoxCollider cameraBoundary;
    [SerializeField] protected DarknessController darknessController;
    [SerializeField] protected EnemyOverworld enemy;
    [SerializeField] protected int stage;
    public float chestTimerDuration;
    public bool isChestUnlocked = false;
    void Start()
    {
        Assert.IsTrue(stage > -1 && stage < 4, gameObject.name + ": Invalid Stage number");
        Assert.IsTrue(buttons.Count == buttons.Distinct().Count(),
            gameObject.name + ": Duplicate Buttons = " + buttons.Distinct().Count() + "/" + buttons.Count);
    }

    // Update is called once per frame
    void Update() {
        if (Time.timeScale != 0 && GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>()._overworldIsActive) {
            PlayerEnterGate();
            if (Input.GetKeyDown(KeyCode.O) && GetChamberIsActive()) {
                ResetChamberObjects();
            }
        }
    }

    public void ResetChamberObjects() {
        foreach (Transform child in transform) {
            ResettableObject resettable = child.gameObject.GetComponent<ResettableObject>();
            if (resettable) {
                resettable.ResetObject();
            }
        }

        GameObject.FindWithTag("PlayerCharacter").GetComponent<CharacterBehavior>().ResetObject();
    }

    public bool GetChamberIsActive() {
        return chamberBoundary.GetChamberIsActive();
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
            if (darknessController) {
                GameObject.FindWithTag("Darkness").GetComponent<SpriteRenderer>().enabled =
                    darknessController.GetIsActive();
            } else {
                GameObject.FindWithTag("Darkness").GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    protected virtual void PlayerEnterGate() {
        foreach (GateArea gate in gates) {
            if (gate.GetIsPlayerNearby() && Input.GetKeyDown(KeyCode.E) && GetChamberComplete()
                && GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>()._overworldIsActive) {
                ProceedChamber();
                return;
            }
        }
    }

    public virtual bool GetChamberComplete() {
        if (buttons.Count == 0 && !enemy) return true;
        if (buttons.Count > 0) {
            foreach (BasicButton button in buttons) {
                if (!button.GetIsPushed()) {
                    return false;
                }
            }
        }

        if (!enemy) return true;
        if (!enemy.isCompleted) {
            return false;
        }

        return true;
    }

    protected virtual void ProceedChamber() {
        GameObject player = GameObject.FindWithTag("PlayerCharacter");
        Vector3 nextSpawnPoint = GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>().GetNextSpawn(stage);
        player.transform.position = nextSpawnPoint;
        player.GetComponent<CharacterBehavior>().spawnPoint = nextSpawnPoint;
        GameObject.FindWithTag("Menu").GetComponent<Menu>().BeginStageTitleDisplay();
    }

    protected virtual void SetGate() {
        bool before = gateTileMap.gameObject.activeInHierarchy;
        gateTileMap.gameObject.SetActive(!GetChamberComplete());
        bool after = gateTileMap.gameObject.activeInHierarchy;
        if (before != after) {
            if (!after) {
                audioSource.clip = openSound;
                audioSource.Play();
            } else {
                audioSource.clip = closeSound;
                audioSource.Play();
            }
        }
    }
}

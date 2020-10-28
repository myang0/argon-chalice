using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Chamber2 : BaseChamber
{
    // Start is called before the first frame update
    [SerializeField] private Tilemap gateTileMap;
    [SerializeField] private GateArea gate;
    [SerializeField] protected Transform nextSpawnPoint;
    [SerializeField] protected ChamberBoundary chamberBoundary;
    [SerializeField] protected BoxCollider cameraBoundary;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (Time.timeScale != 0) {
            PlayerEnterGate();
        }
    }

    private void FixedUpdate() {
        SetCamera();
        SetGate();
    }

    private void SetCamera() {
        if (chamberBoundary.GetChamberIsActive()) {
            GameObject.FindGameObjectWithTag("Camera")
                .GetComponent<CinemachineConfiner>()
                .m_BoundingVolume = cameraBoundary;
        }
    }

    private void PlayerEnterGate() {
        if (gate.GetIsPlayerNearby() && Input.GetKeyDown(KeyCode.E) && GetChamberComplete()) {
            ProceedChamber();
        }
    }

    public bool GetChamberComplete() {
        return false;
    }

    private void ProceedChamber() {
        GameObject.FindWithTag("PlayerCharacter").transform.position = nextSpawnPoint.position;
    }

    private void SetGate() {
        gateTileMap.gameObject.SetActive(GetChamberComplete());
    }
}
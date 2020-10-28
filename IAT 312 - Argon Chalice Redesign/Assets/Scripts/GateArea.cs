using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateArea : MonoBehaviour {
    [SerializeField] private BaseChamber baseChamber;
    private bool isPlayerNearby = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            isPlayerNearby = false;
        }
    }

    public bool GetIsPlayerNearby() {
        return isPlayerNearby;
    }

    public bool GetIsEnabled() {
        return baseChamber.GetChamberComplete();
    }
}

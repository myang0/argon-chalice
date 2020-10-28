using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberBoundary : MonoBehaviour {
    private bool chamberIsActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            chamberIsActive = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            chamberIsActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            chamberIsActive = false;
        }
    }

    public bool GetChamberIsActive() {
        return chamberIsActive;
    }
}

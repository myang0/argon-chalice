using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerPortal : Portal {
    private bool isReady = false;

    private void Update() {
        if (Time.timeScale != 0 && isReady && Input.GetKeyDown(KeyCode.E)
            && GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>()._overworldIsActive) {
            WarpPlayer();
        }
    }

    private void WarpPlayer() {
        GameObject player = GameObject.FindWithTag("PlayerCharacter");
        player.transform.position = new Vector3(linkedPosition.x, linkedPosition.y,
            player.transform.position.z);
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            isReady = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            isReady = false;
        }
    }
}

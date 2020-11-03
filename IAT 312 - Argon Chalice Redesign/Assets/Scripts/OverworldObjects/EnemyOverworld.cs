using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyOverworld : MonoBehaviour {
    public bool isCompleted = false;
    public bool isBattleReady = true;
    private bool _isPlayerNear = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>()._overworldIsActive) {
            if (_isPlayerNear && isBattleReady && Input.GetKeyDown(KeyCode.E)) {
                isBattleReady = false;
                _isPlayerNear = false;
                GameManager.GetInstance().StartBattle(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            _isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            _isPlayerNear = false;
        }
    }

    public void BattleComplete() {
        isCompleted = true;
        this.gameObject.SetActive(false);
    }
}

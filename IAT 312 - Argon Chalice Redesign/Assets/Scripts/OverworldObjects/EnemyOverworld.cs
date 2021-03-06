﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyOverworld : MonoBehaviour {
    public Sprite overworldSprite;
    public Sprite battleSprite;
    public float maxHealth;
    public int ballDamageMin;
    public int ballDamageMax;
    public int pillarDamageMin;
    public int pillarDamageMax;
    public int ballAttackRepeat;
    public int pillarAttackRepeat;
    public float ballAttackRepeatDelay;
    public float pillarAttackRepeatDelay;

    public int spearRepeats;
    public float spearSpeed;
    public float spearDelay;

    public int spikeRepeats;
    public float spikeSpeed;
    public float spikeDelay;

    public int numAttacks;
    
    public bool isCompleted = false;
    public bool isBattleReady = true;
    private bool _isPlayerNear = false;

    public bool isBoss = false;
    public bool isFinalBoss = false;

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
                GameObject.FindGameObjectWithTag("OverworldManager").GetComponent<OverWorldManager>().audioSource.Pause();
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
        GameObject.FindGameObjectWithTag("OverworldManager").GetComponent<OverWorldManager>().audioSource.UnPause();
        this.gameObject.SetActive(false);
    }
}

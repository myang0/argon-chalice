﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _instance = null;
    public static GameManager GetInstance() {
        if (_instance == null) {
            _instance = GameObject.FindObjectOfType<GameManager>();
            if (_instance == null) {
                GameObject container = new GameObject("GameManager");
                _instance = container.AddComponent<GameManager>();
            }
        }

        return _instance;
    }

    public float maxHealth = 150;
    public float health;
    public bool hoverEnabled = false;
    public bool hasRage = false;
    public bool canRevive = false;
    public int humanityValue;
    public int deathCount = 0;
    public EnemyOverworld currentEnemy;
    void Start() {
        health = maxHealth;
    }

    void Update() {
        if (humanityValue > 100) humanityValue = 100;
        if (humanityValue < -100) humanityValue = -100;
    }

    public void StartBattle(EnemyOverworld enemy) {
        GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>().OverworldSetState(false);
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
        currentEnemy = enemy;
    }

    public void EndBattle() {
        GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>().OverworldSetState(true);
        SceneManager.UnloadSceneAsync("BattleScene");
        currentEnemy.BattleComplete();
        currentEnemy = null;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("SpikeTrap")) {
            g.GetComponent<SpikeTrap>().ResetObject();
        }
    }
}

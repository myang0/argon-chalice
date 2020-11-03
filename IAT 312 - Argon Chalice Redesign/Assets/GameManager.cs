using System.Collections;
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
    private EnemyOverworld currentEnemy;
    void Start()
    {
        
    }

    void Update()
    {
        
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
    }
}

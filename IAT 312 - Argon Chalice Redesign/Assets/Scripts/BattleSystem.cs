using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { PLAYER_PHASE, DIALOGUE, ENEMY_PHASE }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    
    private EventText eText;
    private Boss boss;

    private GameObject actionPanel;

    void Start()
    {   
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        eText = GameObject.FindGameObjectWithTag("EventText").GetComponent<EventText>();
        actionPanel = GameObject.FindGameObjectWithTag("ActionPanel");

        StartEnemyPhase();
    }

    public void StartPlayerPhase() {
        state = BattleState.PLAYER_PHASE;
        eText.SetText("What will the hero do?");
        actionPanel.SetActive(true);
    }

    public void StartEnemyPhase() {
        state = BattleState.ENEMY_PHASE;
        eText.SetText("The enemy attacks!");
        actionPanel.SetActive(false);
        boss.EnemyPhaseAction();
    }
}

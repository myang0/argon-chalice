using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { PLAYER_PHASE, DIALOGUE, ENEMY_PHASE, PLAYER_WIN, PLAYER_LOSE }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    
    private EventText eText;
    private Boss boss;

    private UIManager uiManager;

    private GameObject actionPanel;

    void Start()
    {   
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        eText = GameObject.FindGameObjectWithTag("EventText").GetComponent<EventText>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        StartPlayerPhase();
    }

    public void StartPlayerPhase() {
        if (state != BattleState.PLAYER_WIN && state != BattleState.PLAYER_LOSE) {
            state = BattleState.PLAYER_PHASE;
            uiManager.ActivateButtons();
            eText.SetText("What will the Hero do?");
        }
    }

    public void StartEnemyPhase() {
        if (state != BattleState.PLAYER_WIN && state != BattleState.PLAYER_LOSE) {
            state = BattleState.ENEMY_PHASE;
            eText.SetText("The enemy attacks!");
            boss.EnemyPhaseAction();
        }
    }

    public void PlayerWin() {
        state = BattleState.PLAYER_WIN;
        eText.SetText("Enemy defeated!");
        StartCoroutine(EndBattle());
    }

    private IEnumerator EndBattle() {
        yield return new WaitForSeconds(2.5f);
        GameManager.GetInstance().EndBattle();
    }

    public void PlayerLose() {
        state = BattleState.PLAYER_LOSE;
        eText.SetText("You were defeated...");
    }
}

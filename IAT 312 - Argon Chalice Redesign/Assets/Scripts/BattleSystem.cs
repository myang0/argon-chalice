using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { PLAYER_PHASE, DIALOGUE, ENEMY_PHASE, PLAYER_WIN, PLAYER_LOSE }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    [SerializeField] private AudioClip _battleMusic;
    [SerializeField] private AudioClip _bossMusic;
    [SerializeField] private AudioClip _finalBossMusic;

    private AudioSource _audio;
    
    private EventText eText;
    private Boss boss;

    private UIManager uiManager;

    private GameObject actionPanel;

    void Start()
    {   
        _audio = GetComponent<AudioSource>();

        GameManager gm  = GameManager.GetInstance();
        if (gm.currentEnemy.isBoss) {
            _audio.clip = _bossMusic;
        } else if (gm.currentEnemy.isFinalBoss) {
            _audio.clip = _finalBossMusic;
        } else {
            _audio.clip = _battleMusic;
        }
        
        _audio.Play();

        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        eText = GameObject.FindGameObjectWithTag("EventText").GetComponent<EventText>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        actionPanel = GameObject.FindGameObjectWithTag("ActionPanel");
        StartPlayerPhase();
    }

    public void StartPlayerPhase() {
        if (state != BattleState.PLAYER_WIN && state != BattleState.PLAYER_LOSE) {
            state = BattleState.PLAYER_PHASE;
            uiManager.ActivateButtons();
            eText.SetText("What will the Adventurer do?");
            actionPanel.SetActive(true);
        }
    }

    public void StartEnemyPhase() {
        if (state != BattleState.PLAYER_WIN && state != BattleState.PLAYER_LOSE) {
            state = BattleState.ENEMY_PHASE;
            eText.SetText("The enemy attacks!\n\n" +
                          "[LEFT CLICK] Jump\n" +
                          "[RIGHT CLICK] Block");
            boss.EnemyPhaseAction();
        }
    }

    public void PlayerWin() {
        state = BattleState.PLAYER_WIN;
        eText.SetText("Enemy defeated!");
        StartCoroutine(EndBattle());
    }

    public void StartEnemyPhaseOnDelay() {
        StartCoroutine(EnemyPhaseDelay());
    }

    public void PlayerLose() {
        state = BattleState.PLAYER_LOSE;
        eText.SetText("You were defeated...");
    }

    private IEnumerator EndBattle() {
        yield return new WaitForSeconds(1.5f);
        BattlePlayer player = GameObject.FindWithTag("Player").GetComponent<BattlePlayer>();

        _audio.Stop();

        GameManager.GetInstance().health = player.health;
        GameManager.GetInstance().hasRage = player.hasRage;
        GameManager.GetInstance().canRevive = player.canRevive;
        GameManager.GetInstance().hoverEnabled = player.hoverEnabled;
            
        GameManager.GetInstance().EndBattle();
    }

    IEnumerator EnemyPhaseDelay() {
        yield return new WaitForSeconds(2.5f);
        StartEnemyPhase();
    }
}

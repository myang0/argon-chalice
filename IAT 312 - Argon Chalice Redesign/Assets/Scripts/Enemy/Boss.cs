using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private BattleSystem battleSys;
    private EventText eText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject ballProjectile;
    [SerializeField] private GameObject pillarAttack;
    [SerializeField] private GameObject _spearAttack;
    [SerializeField] private GameObject _spikeAttack;
    [SerializeField] private float health;
    private int _ballAttackRepeat;
    private int _pillarAttackRepeat;
    private float _ballAttackRepeatDelay;
    private float _pillarAttackRepeatDelay;
    private GameManager _gameManager = null;

    void Start() {
        _gameManager = GameManager.GetInstance();
        spriteRenderer.sprite = _gameManager.currentEnemy.battleSprite;
        health = _gameManager.currentEnemy.maxHealth;
        ballProjectile.GetComponent<BossBallProjectile>().maxDamage = _gameManager.currentEnemy.ballDamageMax;
        ballProjectile.GetComponent<BossBallProjectile>().minDamage = _gameManager.currentEnemy.ballDamageMin;
        pillarAttack.GetComponent<BossFirePillar>().maxDamage = _gameManager.currentEnemy.pillarDamageMax;
        pillarAttack.GetComponent<BossFirePillar>().minDamage = _gameManager.currentEnemy.pillarDamageMin;
        _ballAttackRepeat = _gameManager.currentEnemy.ballAttackRepeat;
        _pillarAttackRepeat = _gameManager.currentEnemy.pillarAttackRepeat;
        _ballAttackRepeatDelay = _gameManager.currentEnemy.ballAttackRepeatDelay;
        _pillarAttackRepeatDelay = _gameManager.currentEnemy.pillarAttackRepeatDelay;
        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        eText = GameObject.FindGameObjectWithTag("EventText").GetComponent<EventText>();
    }

    void Update()
    {
        transform.position += new Vector3(0, 0.0001f * Mathf.Sin(Time.time), 0);
    }

    void FixedUpdate() {
        
    }

    public void InflictDamage(float damage) {
        eText.gameObject.SetActive(true);
        GameObject.FindWithTag("Player").GetComponent<BattlePlayer>()._isAttacking = true;
        eText.SetText(string.Format("Enemy took {0} damage!", damage));

        Debug.Log(string.Format("Enemy took {0} damage!", damage));
        health -= damage;

        if (health <= 0) {
            RewardChance();
            StartCoroutine(WinDelay());
            // battleSys.PlayerWin();
        }
    }

    private void RewardChance() {
        int giveRewardChance = Random.Range(0, 2);
        if (giveRewardChance != 0) return;

        BattlePlayer player = GameObject.FindWithTag("Player").GetComponent<BattlePlayer>();
        int rewardIndex = Random.Range(0, 3);
        string rewardName;

        if (rewardIndex == 0) {
            if (player.hoverEnabled) return;

            player.hoverEnabled = true;
            rewardName = "Hover Feather! Left click while in the air to fall slower.";
        } else if (rewardIndex == 1) {
            if (player.hasRage) return;

            player.hasRage = true;
            rewardName = "Rage Bandanna! Deal more damage the lower your health is.";
        } else {
            if (player.canRevive) return;

            player.canRevive = true;
            rewardName = "Ankh Charm! You can now cheat death one time.";
        }

        eText.SetText(string.Format("The enemy dropped the {0} You add it to your inventory.", rewardName));
        StartCoroutine(ItemDelay());
    }

    private IEnumerator ItemDelay() {
        yield return new WaitForSeconds(5f);
        battleSys.PlayerWin();
    } 

    private IEnumerator WinDelay() {
        yield return new WaitForSeconds(1.5f);
        battleSys.PlayerWin();
    }

    public void EnemyPhaseAction() {
        // TODO: add more attacks and actions
        int randomAttack = Random.Range(0, 4);

        if (randomAttack == 0) {
            StartCoroutine(ProjectileWave());
        } else if (randomAttack == 1) {
            StartCoroutine(PillarWave());
        } else if (randomAttack == 2) {
            StartCoroutine(SpearWave());
        } else {
            StartCoroutine(SpikeWave());
        }
    }

    IEnumerator ProjectileWave() {
        for (int i = 0; i < _ballAttackRepeat; i++) {
            yield return new WaitForSeconds(_ballAttackRepeatDelay);

            Instantiate(ballProjectile, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(_ballAttackRepeatDelay * _ballAttackRepeat * 0.45f + 1.5f/_ballAttackRepeat);
        // yield return new WaitForSeconds(2);

        battleSys.StartPlayerPhase();
    }

    IEnumerator PillarWave() {
        for (int i = 0; i < _pillarAttackRepeat; i++) {
            Instantiate(pillarAttack, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_pillarAttackRepeatDelay);
        }
        
        yield return new WaitForSeconds(_pillarAttackRepeatDelay * _pillarAttackRepeat * 0.2f);
        battleSys.StartPlayerPhase();
    }

    IEnumerator SpearWave() {
        // TODO: remove hardcoding
        for (int i = 0; i < 5; i++) {
            Instantiate(_spearAttack, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.75f + Random.Range(0f, 0.5f));
        }

        yield return new WaitForSeconds(2);
        battleSys.StartPlayerPhase();
    }

    IEnumerator SpikeWave() {
        // TODO: remove hardcoding
        for (int i = 0; i < 3; i++) {
            float xPos = 2;
            for (int j = 0; j < 12; j++) {
                Vector3 pos = new Vector3(xPos, 1.2f, 0);
                Instantiate(_spikeAttack, pos, Quaternion.identity);
                xPos -= 1f;

                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(1);
        battleSys.StartPlayerPhase();
    }
}

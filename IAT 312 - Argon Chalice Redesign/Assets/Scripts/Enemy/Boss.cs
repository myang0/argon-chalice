using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private BattleSystem battleSys;
    private EventText eText;
    [SerializeField] private GameObject _dmgPopup;

    [SerializeField] private AudioClip _normDmg;
    [SerializeField] private AudioClip _bigDmg;
    private AudioSource _audio;

    [SerializeField] private GenericBar hpBar;
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

    private int _spikeRepeats;
    private float _spikeSpeed;
    private float _spikeDelay;

    private int _spearRepeats;
    private float _spearSpeed;
    private float _spearDelay;

    public bool isBoss;
    public bool isFinalBoss;

    private GameManager _gameManager = null;

    private int _numAttacks;

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

        _spearRepeats = _gameManager.currentEnemy.spearRepeats;
        _spearSpeed = _gameManager.currentEnemy.spearSpeed;
        _spearDelay = _gameManager.currentEnemy.spearDelay;

        _spikeRepeats = _gameManager.currentEnemy.spikeRepeats;
        _spikeSpeed = _gameManager.currentEnemy.spikeSpeed;
        _spikeDelay = _gameManager.currentEnemy.spikeDelay;

        _numAttacks = _gameManager.currentEnemy.numAttacks;
        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        eText = GameObject.FindGameObjectWithTag("EventText").GetComponent<EventText>();
        hpBar.SetMax(health);
        hpBar.SetVal(health);

        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.position += new Vector3(0, 0.0001f * Mathf.Sin(Time.time), 0);
    }

    void FixedUpdate() {
        
    }

    public void InflictDamage(float damage) {
        _audio.clip = (damage < 80) ? _normDmg : _bigDmg;
        _audio.Play();

        eText.gameObject.SetActive(true);

        BattlePlayer p = GameObject.FindWithTag("Player").GetComponent<BattlePlayer>();
        StartCoroutine(p.ItemDelay());
        eText.SetText(string.Format("Enemy took {0} damage!", damage));

        GameObject dpObject = Instantiate(_dmgPopup, transform.position, Quaternion.identity);
        DamagePopup dp = dpObject.GetComponent<DamagePopup>();
        dp.SetText(damage.ToString());

        float shakeMag = (damage / 200f) * 2f;
        StartCoroutine(Shake(0.15f, shakeMag));

        health -= damage;
        hpBar.SetVal(health);
        if (health <= 0) {
            battleSys.state = BattleState.PLAYER_WIN;
            StartCoroutine(Reward());
        }
    }

    private IEnumerator Reward() {
        yield return new WaitForSeconds(2f);
        if (!RewardChance()) {
            StartCoroutine(WinDelay());
        }
    }
    
    private bool RewardChance() {
        int giveRewardChance = Random.Range(0, 2);
        if (giveRewardChance != 0) return false;

        BattlePlayer player = GameObject.FindWithTag("Player").GetComponent<BattlePlayer>();
        int rewardIndex = Random.Range(0, 3);
        string rewardName;

        if (rewardIndex == 0) {
            if (player.hoverEnabled) return false;

            player.hoverEnabled = true;
            rewardName = "Hover Feather! Left click while in the air to fall slower.";
        } else if (rewardIndex == 1) {
            if (player.hasRage) return false;

            player.hasRage = true;
            rewardName = "Rage Bandanna! Deal more damage the lower your health is.";
        } else {
            if (player.canRevive) return false;

            player.canRevive = true;
            rewardName = "Ankh Charm! You can now cheat death one time.";
        }

        eText.SetText(string.Format("The enemy dropped the {0} You add it to your inventory.", rewardName));
        StartCoroutine(ItemDelay());
        return true;
    }

    private IEnumerator ItemDelay() {
        yield return new WaitForSeconds(3.5f);
        battleSys.PlayerWin();
    } 

    private IEnumerator WinDelay() {
        yield return new WaitForSeconds(2f);
        battleSys.PlayerWin();
    }

    public void EnemyPhaseAction() {
        // TODO: add more attacks and actions
        int randomAttack = Random.Range(0, _numAttacks);

        if (randomAttack == 0) {
            StartCoroutine(ProjectileWave());
        } else if (randomAttack == 1) {
            StartCoroutine(PillarWave());
        } else if (randomAttack == 2) {
            StartCoroutine(SpikeRepeat());
        } else {
            StartCoroutine(SpearWave());
        }

        // StartCoroutine(ProjectileWave());
        // StartCoroutine(PillarWave());
        // StartCoroutine(SpearWave());
        // StartCoroutine(SpikeRepeat());
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
        for (int i = 0; i < _spearRepeats; i++) {
            GameObject sObject = Instantiate(_spearAttack, transform.position, Quaternion.identity);
            sObject.GetComponent<BossSpear>().speed = _spearSpeed;

            yield return new WaitForSeconds(_spearDelay + Random.Range(0, 0.25f));
        }

        yield return new WaitForSeconds(2);
        battleSys.StartPlayerPhase();
    }

    IEnumerator SpikeRepeat() {
        for (int i = 0; i < _spikeRepeats; i++) {
            yield return new WaitForSeconds(_spikeDelay);
            StartCoroutine(SpikeWave());
        }

        yield return new WaitForSeconds(2f);
        battleSys.StartPlayerPhase();
    }

    IEnumerator SpikeWave() {
        // TODO: remove hardcoding
        float xPos = 2;
        for (int j = 0; j < 12; j++) {
            Vector3 pos = new Vector3(xPos, 0.25f, 0);
            Instantiate(_spikeAttack, pos, Quaternion.identity);
            xPos -= 1;

            yield return new WaitForSeconds(_spikeSpeed);
        }
    }

    IEnumerator Shake(float duration, float magnitude) {
        Vector3 origPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, origPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = origPos;
    }
}

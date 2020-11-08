using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    private BattleSystem battleSys;
    private EventText eText;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite standSprite;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D bc;

    [SerializeField] private LayerMask floorLayerMask;

    private Boss boss;

    [SerializeField] private float jumpForce = 25f;
    [SerializeField] private float _maxHoverTime = 30;
    [SerializeField] private float _hoverTimer;
    public bool hoverEnabled = false;

    public bool hasRage = false;

    public float maxHealth;
    [SerializeField] private GenericBar hpBar;
    public float health;
    public bool canRevive = false;

    [SerializeField] private GameObject shieldSprite;
    private bool isBlocking = false;
    private bool isBlockRecharging = false;
    public bool _isAttacking;

    void Start()
    {
        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        eText = GameObject.FindGameObjectWithTag("EventText").GetComponent<EventText>();

        maxHealth = GameManager.GetInstance().maxHealth;
        health = GameManager.GetInstance().health;
        hpBar.SetMax(maxHealth);
        hpBar.SetVal(health);

        hoverEnabled = GameManager.GetInstance().hoverEnabled;
        hasRage = GameManager.GetInstance().hasRage;
        canRevive = GameManager.GetInstance().canRevive;

        _hoverTimer = _maxHoverTime;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && CanHover()) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.gravityScale = 0;
            _hoverTimer--;
        } else {
            rb.gravityScale = 14;
        }

        if (Input.GetMouseButtonDown(0) && CanJump()) Jump();
        if (Input.GetMouseButtonDown(1) && CanBlock()) StartCoroutine(Block());

        if (IsGrounded()) {
            _hoverTimer = _maxHoverTime;
        }

        UpdateSprite();
    }

    public void Respawn() {
        health = 100;
        battleSys.state = BattleState.PLAYER_PHASE;
        battleSys.StartPlayerPhase();
        hpBar.SetVal(health);
    }

    private void SetDeathScreen() {
        DeathScreen.State state = GameObject.FindWithTag("RedScreen").GetComponent<DeathScreen>().state;
        if (health > 35) {
            state = DeathScreen.State.Disabled;
        }

        if (health <= 35 && state == DeathScreen.State.Disabled) {
            state = DeathScreen.State.Heartbeat;
        }

        if (health <= 0 && (state == DeathScreen.State.Disabled || state == DeathScreen.State.Heartbeat)) {
            if (canRevive) {
                health = 100;
                hpBar.SetVal(health);
                state = DeathScreen.State.Disabled;

                canRevive = false;
            } else {
                state = DeathScreen.State.StartDeath;
                battleSys.PlayerLose();
            }
        }

        GameObject.FindWithTag("RedScreen").GetComponent<DeathScreen>().state = state;
    }

    private void UpdateSprite() {
        if (_isAttacking) {
            spriteRenderer.sprite = attackSprite;
        } else {
            spriteRenderer.sprite = IsGrounded() ? standSprite : jumpSprite;
        }
    }

    public void InflictDamage(float dmg) {
        dmg = isBlocking ? 0 : dmg;

        health -= Mathf.Floor(dmg);
        hpBar.SetVal(health);
        SetDeathScreen();
    }

    public void Heal(float healValue) {
        health += healValue;
        hpBar.SetVal(health);
        GameManager.GetInstance().humanityValue += (int) (healValue / 2);

        string healText = $"The Adventurer heals for {healValue} HP!\n\n";
       
        if (healValue > 90) {
            healText = healText + "The blissful taste of a delicious meal restores our adventurer's" +
                       " humanity.";
        } else if (healValue > 40) {
            healText = healText + "The comforting taste of a meal reminds our adventurer of home.";
        } else {
            healText = healText + "The taste of food keeps our adventurer's sanity in check.";
        }
        eText.SetText(healText);
        if (health > maxHealth) health = maxHealth;
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool CanHover() {
        return rb.velocity.y < 0 && !IsGrounded() && _hoverTimer > 0 && hoverEnabled;
    }

    private bool CanJump() {
        return IsGrounded() && (battleSys.state == BattleState.ENEMY_PHASE) && !isBlocking;
    }

    private bool CanBlock() {
        return (battleSys.state == BattleState.ENEMY_PHASE) && !isBlockRecharging && !isBlocking;
    }

    private bool IsGrounded() {
        float offset = 0.05f;
        RaycastHit2D rch = Physics2D.Raycast(bc.bounds.center, Vector2.down, bc.bounds.extents.y + offset, floorLayerMask);

        return rch.collider != null;
    }

    IEnumerator Block() {
        isBlocking = true;
        shieldSprite.SetActive(true);
        yield return new WaitForSeconds(0.15f);

        isBlocking = false;
        isBlockRecharging = true;
        shieldSprite.SetActive(false);
        yield return new WaitForSeconds(0.8f);

        isBlockRecharging = false;
    }

    IEnumerator EndPlayerPhase() {
        yield return new WaitForSeconds(2);

        battleSys.StartEnemyPhase();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    private BattleSystem battleSys;
    private EventText eText;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D bc;

    [SerializeField] private LayerMask floorLayerMask;

    private Boss boss;

    [SerializeField] private float jumpForce = 25f;

    [SerializeField] private float baseAttack;

    [SerializeField] private float maxHealth;
    [SerializeField] private GenericBar hpBar;
    private float health;

    [SerializeField] private GameObject shieldSprite;
    private bool isBlocking = false;
    private bool isBlockRecharging = false;

    void Start()
    {
        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        eText = GameObject.FindGameObjectWithTag("EventText").GetComponent<EventText>();

        maxHealth = GameManager.GetInstance().maxHealth;
        health = maxHealth;
        hpBar.SetMax(maxHealth);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanJump()) Jump();

        if (Input.GetMouseButtonDown(1) && CanBlock()) StartCoroutine(Block());
    }

    public void InflictDamage(float dmg) {
        dmg = isBlocking ? 0 : dmg;

        health -= dmg;
        hpBar.SetVal(health);

        if (health <= 0) {
            battleSys.PlayerLose();
        }
    }

    public void Heal(float healValue) {
        health += healValue;
        hpBar.SetVal(health);

        eText.SetText(string.Format("The hero heals for {0} HP!", healValue));

        if (health > healValue) health = maxHealth;
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool CanJump() {
        return IsGrounded() && (battleSys.state == BattleState.ENEMY_PHASE) && !isBlockRecharging && !isBlocking;
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
        yield return new WaitForSeconds(1);

        isBlockRecharging = false;
    }

    IEnumerator EndPlayerPhase() {
        yield return new WaitForSeconds(2);

        battleSys.StartEnemyPhase();
    }
}

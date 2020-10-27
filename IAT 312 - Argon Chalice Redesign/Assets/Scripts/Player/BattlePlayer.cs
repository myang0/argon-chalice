using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    private BattleSystem battleSys;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D bc;

    [SerializeField] private LayerMask floorLayerMask;

    [SerializeField] private ChargeBar cb;

    private Boss boss;

    public bool isCharging = false;
    private float chargeDuration = 0;
    private float maxChargeTime = 300;

    private float jumpForce = 25f;

    [SerializeField] private float baseAttack;

    [SerializeField] private float maxHealth;
    [SerializeField] private GenericBar hpBar;
    private float health;

    [SerializeField] private float maxMana;
    [SerializeField] private GenericBar mpBar;
    private float mana;

    void Start()
    {
        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();

        health = maxHealth;
        hpBar.SetMax(maxHealth);

        mana = maxMana;
        mpBar.SetMax(maxMana);

        cb.SetMax(maxChargeTime);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanJump()) {
            Jump();
        }

        HandleAttackCharge();        
    }

    private void HandleAttackCharge() {
        if (isCharging) {
            chargeDuration++;
            cb.SetVal(chargeDuration);

            if (Input.GetMouseButtonDown(0)) EndAttack(chargeDuration / maxChargeTime);

            if (chargeDuration >= maxChargeTime) EndAttack(0.1f);
        }
    }

    public void StartAttack() {
        isCharging = true;
        cb.Activate();
    }

    private void EndAttack(float dmgMultiplier) {
        isCharging = false;
        boss.InflictDamage(Mathf.Floor(dmgMultiplier * baseAttack));
        chargeDuration = 0;

        cb.SetVal(chargeDuration);
        cb.Deactivate();

        StartCoroutine(EndPlayerPhase());
    }

    public void InflictDamage(float dmg) {
        health -= dmg;
        hpBar.SetVal(health);
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool CanJump() {
        return IsGrounded() && (battleSys.state == BattleState.ENEMY_PHASE);
    }

    private bool IsGrounded() {
        float offset = 0.05f;
        RaycastHit2D rch = Physics2D.Raycast(bc.bounds.center, Vector2.down, bc.bounds.extents.y + offset, floorLayerMask);

        return rch.collider != null;
    }

    IEnumerator EndPlayerPhase() {
        yield return new WaitForSeconds(2);

        battleSys.StartEnemyPhase();
    }
}

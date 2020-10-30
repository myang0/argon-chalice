using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    private BattleSystem battleSys;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D bc;

    [SerializeField] private LayerMask floorLayerMask;

    private Boss boss;

    [SerializeField] private float jumpForce = 25f;

    [SerializeField] private float baseAttack;

    [SerializeField] private float maxHealth;
    [SerializeField] private GenericBar hpBar;
    private float health;

    void Start()
    {
        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();

        health = maxHealth;
        hpBar.SetMax(maxHealth);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanJump()) {
            Jump();
        }
    }

    public void InflictDamage(float dmg) {
        health -= dmg;
        hpBar.SetVal(health);

        if (health <= 0) {
            battleSys.PlayerLose();
        }
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

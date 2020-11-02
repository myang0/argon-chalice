using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirePillar : MonoBehaviour
{
    private float damage;

    void Start() {
        damage = Random.Range(20, 30);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.GetComponent<BattlePlayer>().InflictDamage(damage);
            Destroy(gameObject);
        }
    }
}

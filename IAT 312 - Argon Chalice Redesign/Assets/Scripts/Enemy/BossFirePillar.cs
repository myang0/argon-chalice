using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirePillar : MonoBehaviour
{
    private float damage;
    public float maxDamage;
    public float minDamage;

    void Start() {
        damage = Random.Range(minDamage, maxDamage);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.GetComponent<BattlePlayer>().InflictDamage(damage);
            Destroy(gameObject);
        }
    }
}

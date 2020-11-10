using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirePillar : MonoBehaviour
{
    private float damage;
    public float maxDamage;
    public float minDamage;

    [SerializeField] private GameObject _particles;

    void Start() {
        damage = Random.Range(minDamage, maxDamage);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            Vector3 bottomOfPillar = new Vector3(
                transform.position.x,
                transform.position.y - 1.5f,
                transform.position.z
            );

            Instantiate(_particles, bottomOfPillar, Quaternion.identity);
            col.GetComponent<BattlePlayer>().InflictDamage(damage);
            Destroy(gameObject);
        }
    }
}

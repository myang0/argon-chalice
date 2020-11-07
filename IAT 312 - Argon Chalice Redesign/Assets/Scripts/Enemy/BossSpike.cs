using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpike : MonoBehaviour
{
    private int _damage;
    private float _alpha = 0;
    private bool _isActive = true;

    private SpriteRenderer _sr;
    [SerializeField] private GameObject spawnParticles;

    void Start() {
        _damage =  Random.Range(5, 15);
        _sr = GetComponent<SpriteRenderer>();

        Instantiate(spawnParticles, transform.position, Quaternion.Euler(-90, 0, 0));

        StartCoroutine(DestroyTimer());
    }

    void FixedUpdate() {
        if (_isActive && _alpha < 1) {
            _alpha += 0.3f;
        } else {
            _alpha -= 0.3f;
        }

        _sr.color = new Color(1, 1, 1, _alpha);
        if (_alpha < 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.gameObject.GetComponent<BattlePlayer>().InflictDamage(_damage);
        }
    }

    IEnumerator DestroyTimer() {
        yield return new WaitForSeconds(0.15f);
        _isActive = false;
    }
}

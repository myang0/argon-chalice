using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBallProjectile : MonoBehaviour
{
    private Animator anim;

    private string[] animStates = {"normalMovement", "stutterMovement", "highMovement", "bossBallLoop"};
    public float maxDamage;
    public float minDamage;
    private float damage;

    [SerializeField] private GameObject _particles;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        int randIndex = Random.Range(0, animStates.Length);
        anim.Play(animStates[randIndex]);

        damage = Random.Range(minDamage, maxDamage);
    }

    void FixedUpdate() {
        transform.Rotate(0, 0, 1);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            Instantiate(_particles, transform.position, Quaternion.identity);

            col.GetComponent<BattlePlayer>().InflictDamage(damage);
            Destroy(gameObject);
        }
    }
}

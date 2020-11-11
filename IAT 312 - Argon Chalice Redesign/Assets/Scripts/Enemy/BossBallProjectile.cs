using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBallProjectile : MonoBehaviour
{
    private Animator anim;
    private AudioSource _audio;

    private string[] animStates = {"normalMovement", "stutterMovement", "highMovement", "bossBallLoop"};
    public float maxDamage;
    public float minDamage;
    private float damage;

    private SpriteRenderer _sr;
    private float _alpha = 0;

    [SerializeField] private GameObject _destroyFx;
    [SerializeField] private GameObject _particles;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        _sr = GetComponent<SpriteRenderer>();

        _audio = GetComponent<AudioSource>();
        _audio.Play();

        int randIndex = Random.Range(0, animStates.Length);
        anim.Play(animStates[randIndex]);

        damage = Random.Range(minDamage, maxDamage);
    }

    void Update() {
        if (_alpha < 1f) {
            _sr.color = new Color(1, 1, 1, _alpha);
            _alpha += 0.01f;
        }
    }

    void FixedUpdate() {
        transform.Rotate(0, 0, 1);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            Instantiate(_destroyFx);
            Instantiate(_particles, transform.position, Quaternion.identity);

            col.GetComponent<BattlePlayer>().InflictDamage(damage);
            Destroy(gameObject);
        }
    }
}

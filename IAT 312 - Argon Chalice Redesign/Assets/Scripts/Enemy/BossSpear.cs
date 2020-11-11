using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpear : MonoBehaviour {
    [SerializeField] private AudioClip _appear;
    [SerializeField] private AudioClip _launch;

    private AudioSource _audio;

    private float _rotation = 1;
    private float _alpha = 0;

    private bool _isMovingToPlayer = false;
    private bool _hitPlayer = false;

    private BattlePlayer _player;

    public float baseDamage;
    public float speed;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sr;

    void Start() {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<BattlePlayer>();

        _audio = GetComponent<AudioSource>();
        _audio.PlayOneShot(_appear);

        Vector3 initForce = new Vector3(Random.Range(1.5f, 3f), Random.Range(-3f, 3f), 0);
        _rb.velocity = initForce;

        baseDamage = Random.Range(10, 20);

        StartCoroutine(MoveToPlayerDelay());
    }

    void Update() {
        if (!_isMovingToPlayer) RotateToPlayer(_player.transform.position, transform.position);

        if (_alpha < 1f) {
            _sr.color = new Color(1, 1, 1, _alpha);
            _alpha += 0.01f;
        }
        
        if (Mathf.Abs(transform.position.x - _player.transform.position.x) > 20) {
            Destroy(gameObject);
        }
    }

    void MoveToPlayer(Vector3 playerPos, Vector3 spearPos) {
        Vector3 vectorToPlayer = (playerPos - spearPos).normalized;
        _rb.velocity = vectorToPlayer * speed;
    }

    void RotateToPlayer(Vector3 playerPos, Vector3 spearPos) {
        Vector3 vectorToPlayer = (playerPos - spearPos).normalized;
        float angle = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle + 180);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player") && !_hitPlayer) {
            _player.InflictDamage(baseDamage);
            _hitPlayer = true;
        }
    }

    IEnumerator MoveToPlayerDelay() {
        yield return new WaitForSeconds(1);

        Vector3 playerPos = _player.transform.position;
        Vector3 spearPos = transform.position;

        _audio.PlayOneShot(_launch);

        _isMovingToPlayer = true;
        MoveToPlayer(playerPos, spearPos);
        RotateToPlayer(playerPos, spearPos);
    }
}

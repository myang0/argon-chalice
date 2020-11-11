using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTarget : MonoBehaviour
{
    private MinigameManager _mgm;
    private Rigidbody2D _rb;

    [SerializeField] private GameObject _particles;
    [SerializeField] private GameObject _breakFx;

    private (float fst, float snd) _xBounds;

    void Start() {
        _mgm = GameObject.FindGameObjectWithTag("MinigameManager").GetComponent<MinigameManager>();

        _rb = GetComponent<Rigidbody2D>();

        _rb.velocity = new Vector3(Random.Range(10, 20), 0, 0);
    }

    void Update() {
        float velX = _rb.velocity.x;

        if (transform.position.x < _xBounds.fst) {
            transform.position = new Vector3(_xBounds.fst, transform.position.y, 0);
            _rb.velocity = new Vector3(-velX, 0, 0);
        }

        if (transform.position.x > _xBounds.snd) {
            transform.position = new Vector3(_xBounds.snd, transform.position.y, 0);
            _rb.velocity = new Vector3(-velX, 0, 0);
        }
    }

    public void SetXBounds((float, float) xTuple) {
        _xBounds = xTuple;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Bullet")) {
            _mgm.HitTarget();

            Instantiate(_breakFx);
            Instantiate(_particles, transform.position, Quaternion.identity);

            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }
}

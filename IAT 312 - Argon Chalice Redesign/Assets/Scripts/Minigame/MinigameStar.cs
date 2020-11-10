using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameStar : MonoBehaviour
{
    private MinigameManager mgm;
    private Rigidbody2D rb;

    private float rotationSpeed;

    private (float fst, float snd) xBounds, yBounds;

    [SerializeField] private GameObject _particles;

    void Start() {
        mgm = GameObject.FindGameObjectWithTag("MinigameManager").GetComponent<MinigameManager>();

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(Random.Range(3f, 6f), 0, 0);

        rotationSpeed = Random.Range(-0.5f, 0.5f);
    }

    void Update() {
        transform.Rotate(0, 0, rotationSpeed);

        if (transform.position.x >= xBounds.snd) {
            transform.position = new Vector3(xBounds.fst, transform.position.y, transform.position.z);
        }
    }

    public void SetXBounds((float, float) xTuple) {
        xBounds = xTuple;
    }

    public void SetYBounds((float, float) yTuple) {
        yBounds = yTuple;
    }

    void OnMouseDown() {
        Instantiate(_particles, transform.position, Quaternion.identity);
        mgm.HitStar(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBar : MonoBehaviour
{
    private Rigidbody2D rb;

    private (float fst, float snd) xBounds, yBounds;

    private float targetX;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector3(10f, 0, 0);
    }

    void Update() {
        float velX = rb.velocity.x;

        if (transform.position.x < xBounds.fst) {
            transform.position = new Vector3(xBounds.fst, transform.position.y, 0);
            rb.velocity = new Vector3(-velX, 0, 0);
        }

        if (transform.position.x > xBounds.snd) {
            transform.position = new Vector3(xBounds.snd, transform.position.y, 0);
            rb.velocity = new Vector3(-velX, 0, 0);
        }
    }

    public void SetTarget(float target) {
        targetX = target;
    }

    public void SetXBounds((float, float) xTuple) {
        xBounds = xTuple;
    }

    public void SetYBounds((float, float) yTuple) {
        yBounds = yTuple;
    }

    public float GetDistance() {
        return Mathf.Abs(transform.position.x - targetX);
    }
}

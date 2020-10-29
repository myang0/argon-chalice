using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : ResettableObject {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite puddleWithRock;
    [SerializeField] private Sprite puddleWithout;
    [SerializeField] private BoxCollider2D boxCollider2D;
    public bool hasRock = false;

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("MovableRock") || hasRock
            || other.gameObject.GetComponent<MovableRock>().touchedPuddle) return;

        other.gameObject.GetComponent<MovableRock>().Hide();
        spriteRenderer.sprite = puddleWithRock;
        hasRock = true;
        boxCollider2D.enabled = false;
    }

    public override void ResetObject() {
        spriteRenderer.sprite = puddleWithout;
        hasRock = false;
        boxCollider2D.enabled = true;
    }
}

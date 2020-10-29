using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableRock : ResettableObject {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider2D;
    public bool touchedPuddle = false;
    private Vector3 _spawnLocation;

    private void Start() {
        _spawnLocation = transform.position;
    }

    public override void ResetObject() {
        transform.position = _spawnLocation;
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
    }

    public void Hide() {
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
    }
}

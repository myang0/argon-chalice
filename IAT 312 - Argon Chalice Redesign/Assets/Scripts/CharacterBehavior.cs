using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {
    private Vector2 movementDirection;
    private float movementSpeed = 5f;
    public Vector3 spawnPoint = new Vector3(-6, 2, -5);
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private GameObject interactionButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0) {
            GetPlayerMovement();
        }
    }

    private void FixedUpdate() {
        MovePlayer();
        MoveInteractionButton();
        AnimatePlayer();
        FlipPlayer();
    }

    private void MoveInteractionButton() {
        // interactionButton.transform.position = new Vector3(1, 15, -3);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("PlayerIgnoreCollision")) {
            Physics2D.IgnoreCollision(this.boxCollider, other.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ShowInteractButton(other);
    }

    private void OnTriggerExit2D(Collider2D other) {
        RemoveInteractButton(other);
    }

    private void RemoveInteractButton(Collider2D other) {
        GateArea gate = other.gameObject.GetComponent<GateArea>();
        if (other.gameObject.CompareTag("GateExit") && gate.GetIsEnabled()) {
            interactionButton.SetActive(false);
        }
    }

    private void ShowInteractButton(Collider2D other) {
        GateArea gate = other.gameObject.GetComponent<GateArea>();
        if (other.gameObject.CompareTag("GateExit") && gate.GetIsEnabled()) {
            interactionButton.SetActive(true);
        }
    }

    private void AnimatePlayer() {
        if (IsPlayerMoving()) {
            anim.speed = 1;
        } else {
            anim.Play("PlayerMovement", 0, 0.25f);
            anim.speed = 0;
        }
    }

    private bool IsPlayerMoving() {
        return movementDirection.x != 0 || movementDirection.y != 0;
    }

    private void FlipPlayer() {
        if (!IsPlayerMoving()) return;
        if (movementDirection.normalized.x > 0) {
            sprite.flipX = true;
        } else if (movementDirection.normalized.x < 0) {
            sprite.flipX = false;
        }
    }

    private void MovePlayer() {
        rigidBody.MovePosition(rigidBody.position +
                               movementDirection * (movementSpeed * Time.fixedDeltaTime));
    }

    private void GetPlayerMovement() {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");
    }

    public void ResetSpawn() {
        transform.position = spawnPoint;
    }
}

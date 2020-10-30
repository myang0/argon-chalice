using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : ResettableObject {
    public enum State {Controllable, Pulled, Pushed}
    public State state;
    public Vector2 movementDirection;
    public Vector3 spawnPoint;
    
    private float _movementSpeed = 5f;
    private float _pushSpeed = 10f;
    private float _pushAnimationAngle = 0f;
    
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private GameObject interactionButton;
    void Start() {
        spawnPoint = transform.position;
    }

    void Update()
    {
        
    }

    private void FixedUpdate() {
        if (state == State.Controllable) {
            GetPlayerMovement();
            MovePlayer();
        } else if (state == State.Pushed) {
            PushPlayer();
            AnimatePush();
        }
        Animate();
        FlipPlayer();
    }

    private void AnimatePush() {
        _pushAnimationAngle += 15;
        transform.RotateAround(transform.position, transform.up, Time.deltaTime*360f);
    }

    private void PushPlayer() {
        rigidBody.MovePosition(rigidBody.position +
                               movementDirection * (_pushSpeed * Time.fixedDeltaTime));
    }

    public void OnTrapCollision() {
        Debug.Log("Player: OWWWWW!! SPIKES?!?!?!?");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("PlayerIgnoreCollision")) {
            Physics2D.IgnoreCollision(this.boxCollider, other.collider);
        } else {
            ResetState();
        }
    }

    private void ResetState() {
        state = State.Controllable;
        _pushAnimationAngle = 0;
        transform.rotation = new Quaternion(0, 0, 0, 1);
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

    private void Animate() {
        if (IsPlayerMoving()) {
            anim.speed = 1;
        } else {
            anim.Play("PlayerMovement", 0, 0.25f);
            anim.speed = 0;
        }
    }

    public bool IsPlayerMoving() {
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
                               movementDirection * (_movementSpeed * Time.fixedDeltaTime));
    }

    private void GetPlayerMovement() {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");
    }

    public override void ResetObject() {
        transform.position = spawnPoint;
        interactionButton.SetActive(false);
        ResetState();
    }
}

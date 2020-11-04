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
    private bool _isInvulnerable = false;
    private Coroutine _currentCoroutine = null;
    
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite standSprite;
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
        if (!GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>()
            ._overworldIsActive) return;
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
        if (!_isInvulnerable) {
            TakeDamage(10);
        }
        _currentCoroutine = StartCoroutine(ActivateInvulnerability());
    }

    private void TakeDamage(float damage) {
        GameManager.GetInstance().health -= damage;
        if (GameManager.GetInstance().health < 1) {
            GameManager.GetInstance().health = 1;
        }
        Debug.Log("Player: OW SPIKES??? --- Health = " + GameManager.GetInstance().health);
    }

    private IEnumerator ActivateInvulnerability() {
        _isInvulnerable = true;
        yield return new WaitForSeconds(0.1f);
        _isInvulnerable = false;
        _currentCoroutine = null;
    }

    private IEnumerator DelayReset() {
        yield return new WaitForSeconds(0.2f);
        ResetState();
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
        if (_currentCoroutine != null) {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("GateExit")) {
            GateArea gate = other.gameObject.GetComponent<GateArea>();
            interactionButton.SetActive(gate.GetIsEnabled());
        } else if (other.gameObject.CompareTag("PlayerPortal")) {
            interactionButton.SetActive(true);
        } else if (other.gameObject.CompareTag("Chest") && !other.gameObject.GetComponent<Chest>().isOpened &&
                   other.gameObject.GetComponent<Chest>().isUnlocked) {
            interactionButton.SetActive(true);
        } else if (other.gameObject.CompareTag("EnemyOverworld") &&
                   other.gameObject.GetComponent<EnemyOverworld>().isBattleReady) {
            interactionButton.SetActive(true);
        } else if (other.gameObject.CompareTag("Item") && !other.gameObject.GetComponent<BaseItem>().isPickedUp) {
            interactionButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("GateExit")) {
            GateArea gate = other.gameObject.GetComponent<GateArea>();
            interactionButton.SetActive(false);
        } else if (other.gameObject.CompareTag("PlayerPortal")) {
            interactionButton.SetActive(false);
        } else if (other.gameObject.CompareTag("Chest")) {
            interactionButton.SetActive(false);
        } else if (other.gameObject.CompareTag("EnemyOverworld")) {
            interactionButton.SetActive(false);
        } else if (other.gameObject.CompareTag("Item")) {
            interactionButton.SetActive(false);
        }
    }

    public void DeactivateInteractionButton() {
        interactionButton.SetActive(false);
    }

    public void ActivateChest() {
        interactionButton.SetActive(false);
        Debug.Log("Opened chest!");
    }

    private void Animate() {
        if (IsPlayerMoving())
        {
            anim.enabled = true;
            //anim.speed = 1;
        } else
        {
            anim.enabled = false;
            sprite.sprite = standSprite;
            //anim.Play("PlayerMovement", 0, 0.25f);
            //anim.speed = 0;
        }
    }

    public bool IsPlayerMoving() {
        return movementDirection.x != 0 || movementDirection.y != 0;
    }

    private void FlipPlayer() {
        if (!IsPlayerMoving()) return;
        if (movementDirection.normalized.x > 0) {
            sprite.flipX = false;
        } else if (movementDirection.normalized.x < 0) {
            sprite.flipX = true;
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
        ResetState();
        transform.position = spawnPoint;
        interactionButton.SetActive(false);
    }
}

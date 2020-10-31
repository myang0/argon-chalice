using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions;

public class SpikeTrap : ResettableObject {
    [SerializeField] private Sprite activeSpike;
    [SerializeField] private Sprite inActiveSpike;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private float intervalDelay;
    [SerializeField] private float startDelay;
    [SerializeField] private bool buttonToggleable;
    [SerializeField] private bool allButtonsMustBeActive;
    [SerializeField] private List<BasicButton> buttons = new List<BasicButton>();
    public bool _started = false;
    public bool _isActive = false;
    private Coroutine _currentCoroutine = null;
    // Start is called before the first frame update
    void Start() {
        Initialize();
    }

    private void Initialize() {
        if (buttonToggleable) {
            Assert.IsTrue(buttons.Count > 0,
                gameObject.name + " [ERROR] No buttons are attached to this object.");
        }
        
        if (startDelay == 0) {
            _started = true;
        } else {
            _currentCoroutine = StartCoroutine(ActivateTrap());
        }
    }

    private IEnumerator ActivateTrap() {
        yield return new WaitForSeconds(startDelay);
        _started = true;
        _currentCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        SetSprite();
        SetCollider();
        if (_started) {
            if (buttonToggleable) {
                _isActive = ButtonToggle();
            } else {
                IntervalActivation();
            }
        }
    }

    private void SetCollider() {
        boxCollider2D.enabled = _isActive;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!_isActive) return;
        if (other.gameObject.CompareTag("PlayerCharacter")) {
            other.gameObject.GetComponent<CharacterBehavior>().OnTrapCollision();
        }
    }

    private void IntervalActivation() {
        if (_currentCoroutine != null) return;
        _currentCoroutine = StartCoroutine(IntervalCoroutine());
    }

    private IEnumerator IntervalCoroutine() {
        yield return new WaitForSeconds(intervalDelay);
        _isActive = !_isActive;
        _currentCoroutine = null;
    }

    private bool ButtonToggle() {
        if (allButtonsMustBeActive) {
            foreach (BasicButton button in buttons) {
                if (!button.GetIsPushed()) {
                    return false;
                }
            }

            return true;
        } else {
            foreach (BasicButton button in buttons) {
                if (button.GetIsPushed()) {
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSprite() {
        spriteRenderer.sprite = _isActive ? activeSpike : inActiveSpike;
    }

    public override void ResetObject() {
        _isActive = false;
        if (_currentCoroutine != null) {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = null;
        _started = false;
        Initialize();
    }
}

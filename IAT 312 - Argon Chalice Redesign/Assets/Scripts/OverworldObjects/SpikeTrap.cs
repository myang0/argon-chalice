using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : ResettableObject {
    [SerializeField] private Sprite activeSpike;
    [SerializeField] private Sprite inActiveSpike;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private float intervalDelay;
    [SerializeField] private bool buttonToggleable;
    [SerializeField] private bool allButtonsMustBeActive;
    [SerializeField] private List<BasicButton> buttons = new List<BasicButton>();
    private bool _isActive = false;
    private Coroutine _currentCoroutine = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        SetSprite();
        SetCollider();
        if (buttonToggleable) {
            _isActive = ButtonToggle();
        } else {
            IntervalActivation();
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
        StopCoroutine(_currentCoroutine);
        _currentCoroutine = null;
    }
}

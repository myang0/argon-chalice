﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class SpikeTrap : ResettableObject {
    [SerializeField] private Sprite activeSpike;
    [SerializeField] private Sprite inActiveSpike;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D tileCollider;
    [SerializeField] private PolygonCollider2D spikeCollider;
    [SerializeField] private float intervalDelay;
    [SerializeField] private float startDelay;
    [SerializeField] private bool buttonToggleable;
    [SerializeField] private bool allButtonsMustBeActive;
    [SerializeField] private List<BasicButton> buttons = new List<BasicButton>();
    public bool started = false;
    public bool isActive = false;
    private Coroutine _activateTrapCoroutine = null;
    private Coroutine _activateColliderCoroutine = null;
    // Start is called before the first frame update
    void Start() {
        Initialize();
    }

    private void Initialize() {
        if (buttonToggleable) {
            Assert.IsTrue(buttons.Count > 0,
                gameObject.name + " [ERROR] No buttons are attached to this object.");
            Assert.IsTrue(buttons.Count == buttons.Distinct().Count(),
                gameObject.name + ": [ERROR] Duplicate Buttons = " + buttons.Distinct().Count() + "/" + buttons.Count);
        }
        
        if (startDelay == 0 && !buttonToggleable) {
            started = true;
        } else {
            _activateTrapCoroutine = StartCoroutine(ActivateTrap());
        }
    }

    private IEnumerator ActivateTrap() {
        yield return new WaitForSeconds(startDelay);
        started = true;
        _activateTrapCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        SetSprite();
        SetCollider();
        if (started) {
            if (buttonToggleable) {
                isActive = !ButtonToggle();
            } else {
                IntervalActivation();
            }
        }
    }

    private void SetCollider() {
        if (isActive) {
            if (!spikeCollider.enabled && !tileCollider.enabled) {
                spikeCollider.enabled = true;
                StartCoroutine(ActivateColliderCoroutine());
            } else if (!spikeCollider.enabled && tileCollider.enabled) {
                tileCollider.enabled = true;
            }
        } else {
            spikeCollider.enabled = false;
            tileCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isActive) return;
        if (other.CompareTag("PlayerCharacter")) {
            other.gameObject.GetComponent<CharacterBehavior>().OnTrapCollision();
        }
    }

    private void IntervalActivation() {
        if (_activateTrapCoroutine != null) return;
        _activateTrapCoroutine = StartCoroutine(IntervalCoroutine());
    }

    private IEnumerator IntervalCoroutine() {
        yield return new WaitForSeconds(intervalDelay);
        isActive = !isActive;
        _activateTrapCoroutine = null;
    }

    private IEnumerator ActivateColliderCoroutine() {
        yield return new WaitForSeconds(0.05f);
        spikeCollider.enabled = false;
        tileCollider.enabled = true;
        _activateColliderCoroutine = null;
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
        spriteRenderer.sprite = isActive ? activeSpike : inActiveSpike;
    }

    public override void ResetObject() {
        isActive = false;
        if (_activateTrapCoroutine != null) {
            StopCoroutine(_activateTrapCoroutine);
        }

        if (_activateColliderCoroutine != null) {
            StopCoroutine(_activateColliderCoroutine);
        }
        _activateTrapCoroutine = null;
        _activateColliderCoroutine = null;
        started = false;
        Initialize();
    }
}

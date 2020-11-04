using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ToggleableWall : ResettableObject
{
    [SerializeField] private List<BasicButton> buttons = new List<BasicButton>();
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Sprite activeWall;
    [SerializeField] private Sprite inActiveWall;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool allButtonsMustBeActive;
    [SerializeField] private bool inverted;
    [SerializeField] private AudioClip doorOpenAudio;
    [SerializeField] private AudioClip doorCloseAudio;
    [SerializeField] private AudioSource audioSource;
    private bool _isActive = false;

    private void Start() {
        SetMute();
        Assert.IsTrue(buttons.Count > 0,
            gameObject.name + " [ERROR] No buttons are attached to this object.");
        
        Assert.IsTrue(buttons.Count == buttons.Distinct().Count(),
            gameObject.name + ": [ERROR] Duplicate Buttons = " + buttons.Distinct().Count() + "/" + buttons.Count);
    }

    private void FixedUpdate() {
        SetMute();
        SetState();
        SetSprite();
        SetCollider();
    }

    private void SetMute() {
        audioSource.mute = !transform.parent.parent.GetComponent<BaseChamber>().GetChamberIsActive();
    }

    private void SetState() {
        bool prevState = _isActive;
        if (inverted) {
            _isActive = !SetActive();
        } else {
            _isActive = SetActive();
        }

        if (prevState != _isActive) {
            playOpenDoorAudio(_isActive);
        }
    }

    private void playOpenDoorAudio(bool value) {
        if (value) {
            audioSource.clip = doorOpenAudio;
        } else {
            audioSource.clip = doorCloseAudio;
        }
        audioSource.Play();
    }

    private bool SetActive() {
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

    private void SetCollider() {
        boxCollider2D.enabled = !_isActive;
    }

    private void SetSprite() {
        spriteRenderer.sprite = !_isActive ? activeWall : inActiveWall;
    }

    public override void ResetObject() {
        _isActive = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleableWall : ResettableObject
{
    [SerializeField] private List<BasicButton> buttons = new List<BasicButton>();
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Sprite activeWall;
    [SerializeField] private Sprite inActiveWall;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool allButtonsMustBeActive;
    private bool isActive = false;

    private void FixedUpdate() {
        isActive = SetActive();
        SetSprite();
        SetCollider();
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
        boxCollider2D.enabled = !isActive;
    }

    private void SetSprite() {
        spriteRenderer.sprite = !isActive ? activeWall : inActiveWall;
    }

    public override void ResetObject() {
        isActive = true;
    }
}

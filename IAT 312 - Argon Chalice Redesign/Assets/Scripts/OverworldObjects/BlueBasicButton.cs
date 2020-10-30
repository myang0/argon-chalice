using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBasicButton : BasicButton {
    [SerializeField] private Sprite activeUnpushedSprite;
    [SerializeField] private Sprite activePushedSprite;
    private bool _isActive = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
        _isActive = !_isActive;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!isPushed) return;
        isPushed = false;
    }

    protected override void SetSprite() {
        if (_isActive) {
            spriteRenderer.sprite = isPushed ? activePushedSprite : activeUnpushedSprite;
        } else {
            spriteRenderer.sprite = isPushed ? pushedSprite : unpushedSprite;
        }
    }
    
    public override void ResetObject() {
        isPushed = false;
        _isActive = false;
    }

    public override bool GetIsPushed() {
        return _isActive;
    }
}

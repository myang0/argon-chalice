using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicButton : ResettableObject {
    [SerializeField] protected Sprite unpushedSprite;
    [SerializeField] protected Sprite pushedSprite;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected bool isPushed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        SetSprite();
    }

    protected virtual void SetSprite() {
        spriteRenderer.sprite = isPushed ? pushedSprite : unpushedSprite;
    }

    public virtual bool GetIsPushed() {
        return isPushed;
    }

    public override void ResetObject() {
        isPushed = false;
    }
}

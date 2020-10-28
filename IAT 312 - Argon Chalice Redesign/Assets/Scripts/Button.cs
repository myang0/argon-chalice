using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    [SerializeField] protected Sprite unpushedSprite;
    [SerializeField] protected Sprite pushedSprite;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected bool isPushed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        SetSprite();
    }

    private void SetSprite() {
        spriteRenderer.sprite = isPushed ? pushedSprite : unpushedSprite;
    }

    public bool GetIsPushed() {
        return isPushed;
    }
}

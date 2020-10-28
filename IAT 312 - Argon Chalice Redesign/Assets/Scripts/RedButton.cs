using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour {
    [SerializeField] private Sprite unpushedSprite;
    [SerializeField] private Sprite pushedSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isPushed = false;
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

    private void OnTriggerEnter2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!isPushed) return;
        isPushed = false;
    }

    public bool GetIsPushed() {
        return isPushed;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBasicButton : BasicButton {
    private void OnTriggerEnter2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
    }
}
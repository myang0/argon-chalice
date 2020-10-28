using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : Button
{
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
}

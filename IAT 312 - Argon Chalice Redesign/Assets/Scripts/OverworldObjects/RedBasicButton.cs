using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBasicButton : BasicButton {
    [SerializeField] private AudioClip unPressedSound;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
        audioSource.clip = pressedSound;
        audioSource.Play();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
        audioSource.clip = pressedSound;
        audioSource.Play();
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!isPushed) return;
        isPushed = false;
        audioSource.clip = unPressedSound;
        audioSource.Play();
    }
}

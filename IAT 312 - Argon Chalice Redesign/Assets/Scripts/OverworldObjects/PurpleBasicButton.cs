using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBasicButton : BasicButton {
    [SerializeField] private AudioClip unPressedSound;
    [SerializeField] private float delay;
    private Coroutine _resetCoroutine = null;

    private void OnTriggerEnter2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
        audioSource.clip = pressedSound;
        audioSource.Play();
        _resetCoroutine = StartCoroutine(StartResetTimer());
    }

    private IEnumerator StartResetTimer() {
        yield return new WaitForSeconds(delay);
        _resetCoroutine = null;
        ResetObject();
    }

    public override void ResetObject() {
        isPushed = false;
        audioSource.clip = unPressedSound;
        audioSource.Play();
        if (_resetCoroutine != null) {
            StopCoroutine(_resetCoroutine);
        }
    }
}

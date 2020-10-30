using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleButton : BasicButton {
    [SerializeField] private float delay;
    private Coroutine _resetCoroutine = null;

    private void OnTriggerEnter2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
        _resetCoroutine = StartCoroutine(StartResetTimer());
    }

    private IEnumerator StartResetTimer() {
        yield return new WaitForSeconds(delay);
        _resetCoroutine = null;
        ResetObject();
    }

    public override void ResetObject() {
        isPushed = false;
        if (_resetCoroutine != null) {
            StopCoroutine(_resetCoroutine);
        }
    }
}

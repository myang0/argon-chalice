using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PurpleBasicButton : BasicButton {
    [SerializeField] private AudioClip unPressedSound;
    [SerializeField] private AudioClip tickSound;
    [SerializeField] private AudioClip tockSound;
    [SerializeField] private float delay;
    private Coroutine _resetCoroutine = null;
    private float timeElasped = 0f;
    private bool isTick = true;

    private void OnTriggerEnter2D(Collider2D other) {
        if (isPushed) return;
        isPushed = true;
        audioSource.clip = pressedSound;
        audioSource.Play();
        _resetCoroutine = StartCoroutine(StartResetTimer());
    }

    private IEnumerator StartResetTimer() {
        while (timeElasped < delay) {
            Debug.Log(timeElasped);
            float tickDelay = (delay - timeElasped) / delay;
            float minDelay = 0.1f;
            if (tickDelay < minDelay) {
                tickDelay = minDelay;
            }
            yield return new WaitForSeconds(tickDelay);
            timeElasped += tickDelay;
            TickTock();
            // if (timeElasped > 4.9) {
            //     timeElasped = delay;
            // }
        }
        
        // yield return new WaitForSeconds(delay);
        _resetCoroutine = null;
        timeElasped = 0;
        ResetObject();
    }

    private void TickTock() {
        audioSource.clip = isTick ? tickSound : tockSound;
        audioSource.Play();
        isTick = !isTick;
    }
    
    // private IEnumerator CountdownEvent() {
    //     float timeValue = 0.05f;
    //     while (slider.value > 0) {
    //         yield return new WaitForSeconds(timeValue);
    //         slider.value -= timeValue;
    //         fill.color = Color.Lerp(Color.red, Color.green, slider.value/slider.maxValue);
    //         RotateChest();
    //         if (slider.value <= 0) {
    //             isAvailable = false;
    //             chest.sprite = chestLocked;
    //         }
    //         yield return null;
    //     }
    // }

    public override void ResetObject() {
        timeElasped = 0;
        isPushed = false;
        audioSource.clip = unPressedSound;
        audioSource.Play();
        if (_resetCoroutine != null) {
            StopCoroutine(_resetCoroutine);
        }
    }
}

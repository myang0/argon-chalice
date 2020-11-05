using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ChestUnlockTimer : MonoBehaviour {
    [SerializeField] protected float timeDecreaseMultiplier;
    [SerializeField] private Sprite chestUnlocked;
    [SerializeField] private Sprite chestLocked;
    [SerializeField] private Sprite chestOpened;
    [SerializeField] private Image chest;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    public bool isAvailable = true;
    public BaseChamber chamber;
    private bool _isStarted = false;
    private Coroutine _currentCoroutine;
    void Start() {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate() {
        if (!chamber || !chamber.GetChamberIsActive()) {
            chamber = GetCurrentChamber();
            _isStarted = false;
            chest.sprite = chestUnlocked;
            if (_currentCoroutine != null) {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
        }

        if (!chamber) return;
        if (chamber.GetChamberIsActive() && !_isStarted) {
            if (chamber.chestTimerDuration > 0) {
                fill.enabled = true;
                chest.enabled = true;
                slider.maxValue = chamber.chestTimerDuration;
                slider.value = slider.maxValue;
                _currentCoroutine = StartCoroutine(CountdownEvent());
                _isStarted = true;
            } else {
                fill.enabled = false;
                chest.enabled = false;
                chamber.isChestUnlocked = chamber.GetChamberComplete();
            }
        } else if (chamber.GetChamberIsActive()) {
            if (chamber.GetChamberComplete() && isAvailable) {
                chamber.isChestUnlocked = true;
                chest.transform.localRotation = Quaternion.identity;
                if (chest.sprite != chestLocked) {
                    chest.sprite = chestOpened;
                }
                if (_currentCoroutine != null) {
                    StopCoroutine(_currentCoroutine);
                    _currentCoroutine = null;
                }
            } else {
                chamber.isChestUnlocked = false;
                if (chest.sprite != chestLocked) {
                    chest.sprite = chestUnlocked;
                }
                if (_currentCoroutine == null) {
                    _currentCoroutine = StartCoroutine(CountdownEvent());
                }
            }
        }
    }

    private IEnumerator CountdownEvent() {
        float timeValue = 0.05f;
        while (slider.value > 0) {
            yield return new WaitForSeconds(timeValue);
            slider.value -= timeValue;
            fill.color = Color.Lerp(Color.red, Color.green, slider.value/slider.maxValue);
            RotateChest();
            if (slider.value <= 0) {
                isAvailable = false;
                chest.sprite = chestLocked;
            }
            yield return null;
        }
    }

    private void RotateChest() {
        Quaternion a = Quaternion.Euler(new Vector3(0.0f, 0.0f, -25.0f));
        Quaternion b = Quaternion.Euler( new Vector3(0.0f, 0.0f, 25.0f));

        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * 4f));
        chest.transform.localRotation = Quaternion.Lerp(a, b, lerp);
    }

    private BaseChamber GetCurrentChamber() {
        return GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>().GetCurrentChamber();
    }
}

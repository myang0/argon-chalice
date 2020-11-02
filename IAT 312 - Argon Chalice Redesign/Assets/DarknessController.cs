using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessController : MonoBehaviour
{
    [SerializeField] List<BasicButton> buttons = new List<BasicButton>();
    [SerializeField] private bool allButtonsMustBeActive;

    private bool _isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        _isActive = !SetActive();
    }

    private bool SetActive() {
        if (allButtonsMustBeActive) {
            foreach (BasicButton button in buttons) {
                if (!button.GetIsPushed()) {
                    return false;
                }
            }

            return true;
        } else {
            foreach (BasicButton button in buttons) {
                if (button.GetIsPushed()) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool GetIsActive() {
        return _isActive;
    }
}

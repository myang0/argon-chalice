using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedScreen : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private float _opacityValue = 0;
    // Start is called before the first frame update
    public enum State {
        Disabled, Heartbeat, StartDeath, EndDeath
    }

    public bool _opacityRising = false;
    public State state;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Heartbeat) {
            HeartbeatEffect();
        } else if (state == State.Disabled) {
            _opacityValue = 0;
        }

        var color = image.color;
        color = new Color(color.r, color.g, color.b, _opacityValue);
        image.color = color;
    }

    private void HeartbeatEffect() {
        if (_opacityRising) {
            _opacityValue += 0.01f;
            if (_opacityValue > 0.4f) {
                _opacityRising = false;
            }
        } else {
            _opacityValue -= -0.01f;
            if (_opacityValue < 0f) {
                _opacityRising = true;
            }
        }
    }
}

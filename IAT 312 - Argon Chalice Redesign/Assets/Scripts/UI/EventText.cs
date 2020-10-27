using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventText : MonoBehaviour
{
    private Text text;

    void Start() {
        text = GetComponent<Text>();
    }

    public void SetText(string newText) {
        text.text = newText;
    }
}

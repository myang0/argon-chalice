using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericBar : MonoBehaviour
{
    [SerializeField] private Slider s;

    void Start() {
        
    }

    public void SetMax(float val) {
        s.maxValue = val;
        s.value = val;
    }

    public void SetVal(float val) {
        s.value = val;
    }
}

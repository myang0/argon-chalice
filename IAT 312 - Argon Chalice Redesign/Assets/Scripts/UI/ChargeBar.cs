using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    [SerializeField] private Slider s;

    // private BattlePlayer p;

    void Start() {
        // p = GameObject.FindGameObjectWithTag("Player").GetComponent<BattlePlayer>();
        gameObject.SetActive(false);
    }

    public void Activate() {
        gameObject.SetActive(true);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }

    public void SetMax(float val) {
        s.maxValue = val;
        s.value = 0;
    }

    public void SetVal(float val) {
        s.value = val;
    }
}

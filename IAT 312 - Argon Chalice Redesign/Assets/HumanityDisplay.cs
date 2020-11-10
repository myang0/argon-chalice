using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanityDisplay : MonoBehaviour {
    [SerializeField] private Text humanityValue;
    [SerializeField] private Text humanityDesc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        GameManager gameManager = GameManager.GetInstance();
        humanityValue.text = "" + gameManager.humanityValue;
        if (gameManager.humanityValue > -1) {
            humanityValue.color = Color.green;
            humanityDesc.color = Color.green;
        } else {
            humanityValue.color = Color.red;
            humanityDesc.color = Color.red;
        }
    }
}

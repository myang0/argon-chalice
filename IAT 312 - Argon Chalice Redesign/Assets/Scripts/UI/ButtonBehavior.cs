using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    private BasicButton _basicButton;

    private BattlePlayer player;
    private BattleSystem battleSys;

    void Start() {
        _basicButton = GetComponent<BasicButton>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BattlePlayer>();
        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
    }

    void Update() {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : BaseItem
{
    [SerializeField] private int healValue;
    [SerializeField] private string name;

    public override void Use() {
        GameObject bpObject = GameObject.FindGameObjectWithTag("Player");

        if (bpObject == null) return;

        BattlePlayer bp = bpObject.GetComponent<BattlePlayer>();
        bp.Heal(healValue);
    }

    public override string GetName() {
        return name;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageItem : BaseItem
{
    [SerializeField] private int damageValue;

    public override void Use() {
        // GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");

        // if (bossObject == null) return;

        // Boss b = bossObject.GetComponent<Boss>();
        // b.InflictDamage(damageValue);
        Debug.Log("Damage!");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    public virtual void ResetObject() {
        Debug.Log("Resettable Object: Please Override This.");
    }
}

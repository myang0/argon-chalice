using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChildrenZtoZero : ResettableObject
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform) {
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ResetObject() {
        foreach (Transform child in transform) {
            ResettableObject resettable = child.gameObject.GetComponent<ResettableObject>();
            if (resettable) {
                resettable.ResetObject();
            }
        }
    }
}

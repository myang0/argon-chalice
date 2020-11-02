using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChildrenZtoZero : MonoBehaviour
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
}

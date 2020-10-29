using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    [SerializeField] protected GameObject linkedPortal;
    protected Vector3 linkedPosition;
    public List<GameObject> warpedObjects = new List<GameObject>();
    void Start()
    {
        if (linkedPortal) {
            linkedPosition = linkedPortal.transform.position;
        }
    }

    private void FixedUpdate() {
        gameObject.transform.RotateAround(transform.position,
            transform.forward, Time.deltaTime*360f);
    }

    protected IEnumerator AllowWarpingAgain(GameObject other) {
        yield return new WaitForSeconds(0.5f);
        warpedObjects.Remove(other.gameObject);
    }

    protected bool IsAlreadyWarped(GameObject target) {
        return warpedObjects.Contains(target);
    }
}

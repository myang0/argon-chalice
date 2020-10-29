using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPortal : Portal
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (!linkedPortal) return;
        if (other.CompareTag("MovableRock") && !IsAlreadyWarped(other.gameObject)) {
            GameObject otherObj = other.gameObject;
            otherObj.transform.position = new Vector3(linkedPosition.x, linkedPosition.y,
                otherObj.transform.position.z);
            warpedObjects.Add(otherObj);
            linkedPortal.GetComponent<Portal>().warpedObjects.Add(otherObj);
            Debug.Log(warpedObjects.Count);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!linkedPortal) return;
        if (other.CompareTag("MovableRock") && IsAlreadyWarped(other.gameObject)) {
            StartCoroutine(AllowWarpingAgain(other.gameObject));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPortal : Portal
{
    public List<GameObject> warpedObjects = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other) {
        if (!linkedPortal) return;
        if (other.CompareTag("MovableRock") && !IsAlreadyWarped(other.gameObject)) {
            GameObject otherObj = other.gameObject;
            otherObj.transform.position = new Vector3(linkedPosition.x, linkedPosition.y,
                otherObj.transform.position.z);
            warpedObjects.Add(otherObj);
            linkedPortal.GetComponent<ObjectPortal>().warpedObjects.Add(otherObj);
            audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!linkedPortal) return;
        if (other.CompareTag("MovableRock") && IsAlreadyWarped(other.gameObject)) {
            StartCoroutine(AllowWarpingAgain(other.gameObject));
        }
    }
    
    private IEnumerator AllowWarpingAgain(GameObject other) {
        yield return new WaitForSeconds(0.5f);
        warpedObjects.Remove(other.gameObject);
    }

    private bool IsAlreadyWarped(GameObject target) {
        return warpedObjects.Contains(target);
    }
}

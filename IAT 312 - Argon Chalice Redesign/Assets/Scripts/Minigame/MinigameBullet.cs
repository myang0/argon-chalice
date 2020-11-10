using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBullet : MonoBehaviour
{
    private (float fst, float snd) _xBounds, _yBounds;

    void Start() {
        StartCoroutine(DestroyDelay());
    }

    public void SetBounds((float, float) xTuple, (float, float) yTuple) {
        _xBounds = xTuple;
        _yBounds = yTuple;
    }

    IEnumerator DestroyDelay() {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}

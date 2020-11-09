using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Portal : MonoBehaviour {
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected GameObject linkedPortal;
    protected Vector3 linkedPosition;
    void Start() {
        Assert.IsNotNull(linkedPortal,
            gameObject.name + " [ERROR] No linked portal has been selected.");
        linkedPosition = linkedPortal.transform.position;
    }

    private void FixedUpdate() {
        gameObject.transform.RotateAround(transform.position,
            transform.forward, Time.deltaTime*360f);
    }
}

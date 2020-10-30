using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour {
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        transform.position = player.transform.position;
    }
}

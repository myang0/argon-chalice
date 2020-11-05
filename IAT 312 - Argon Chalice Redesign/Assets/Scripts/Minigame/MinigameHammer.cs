using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameHammer : MonoBehaviour {
    private MinigameManager mgm;

    private float rotationSpeed;
    private float x = 0;
    private bool isInZone = false;

    private Vector3 pos;

    void Start() {
        mgm = GameObject.FindGameObjectWithTag("MinigameManager").GetComponent<MinigameManager>();

        rotationSpeed = Random.Range(0.75f, 1.25f);
        pos = transform.position;
    }

    void Update() {
        transform.Rotate(0, 0, rotationSpeed);

        if (Input.GetMouseButtonDown(0) && isInZone) {
            mgm.ClickSuccess();
        }
    }

    void FixedUpdate() {
        float xOffset = 3.5f * Mathf.Sin(x);
        float yOffset = 0.8f * Mathf.Cos(x);

        transform.position = new Vector3(pos.x + xOffset, pos.y + yOffset, pos.z);

        x += 0.075f;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("TargetZone")) {
            isInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.CompareTag("TargetZone")) {
            isInZone = false;
        }
    }
}

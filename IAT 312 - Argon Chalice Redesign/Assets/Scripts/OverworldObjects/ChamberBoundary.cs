using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChamberBoundary : MonoBehaviour {
    private bool chamberIsActive = false;

    [SerializeField] private bool useDialogue;

    private bool _dialogueActivated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            chamberIsActive = true;
            SceneManager.SetActiveScene(gameObject.scene);
            if (useDialogue && !_dialogueActivated) {
                _dialogueActivated = true;
                StartCoroutine(StartDialogue());
            }
        }
    }

    private IEnumerator StartDialogue() {
        yield return new WaitForSeconds(0.05f);
        GameObject.FindWithTag("Menu").GetComponent<Menu>().LoadNextDialogue();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            chamberIsActive = true;
            SceneManager.SetActiveScene(gameObject.scene);
            if (useDialogue && !_dialogueActivated) {
                _dialogueActivated = true;
                StartCoroutine(StartDialogue());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            chamberIsActive = false;
        }
    }

    public bool GetChamberIsActive() {
        return chamberIsActive;
    }
}

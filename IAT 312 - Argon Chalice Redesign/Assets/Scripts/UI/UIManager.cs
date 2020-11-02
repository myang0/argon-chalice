using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] actButtons;

    public void ActivateButtons() {
        foreach (GameObject ab in actButtons) {
            Button b = ab.GetComponent<Button>();
            b.interactable = true;
        }
    }

    public void DisableButtons() {
        foreach (GameObject ab in actButtons) {
            Button b = ab.GetComponent<Button>();
            b.interactable = false;
        }
    }

    public void ToggleButtonInteractability() {
        foreach (GameObject ab in actButtons) {
            Button b = ab.GetComponent<Button>();
            b.interactable = !b.interactable;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    private Inventory inventory;
    private SpriteRenderer sr;

    private GameObject overworldPlayer;

    public bool isPickedUp = false;
    private bool isPlayerNearby = false;

    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        sr = GetComponent<SpriteRenderer>();

        overworldPlayer = GameObject.FindGameObjectWithTag("PlayerCharacter");

        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        PickupItem();
    }

    public void AddToInventory() {
        inventory.AddItem(this);
        sr.enabled = false;
        isPickedUp = true;

        GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<CharacterBehavior>().DeactivateInteractionButton();
    }

    public virtual void Use() {
        Debug.Log("Base item used!");
    }

    public virtual string GetName() {
        return "Base";
    }

    private void PickupItem() {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isPickedUp) AddToInventory();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("PlayerCharacter")) {
            isPlayerNearby = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Chest : MonoBehaviour {
    [SerializeField] private Sprite chestLockedSprite;
    [SerializeField] private Sprite chestUnlockedSprite;
    [SerializeField] private Sprite chestOpenedSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BaseChamber chamber;
    public bool isUnlocked = false;
    public bool isOpened = false;
    public bool isPlayerNearby = false;

    [SerializeField] private int numLootItems;
    public GameObject[] possibleLoot;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(chamber);
    }

    private void Update() {
        OpenChest();
    }

    private void FixedUpdate() {
        SetSprite();
        isUnlocked = chamber.isChestUnlocked;
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
    
    private void OpenChest() {
        if (!isOpened && isPlayerNearby && Input.GetKeyDown(KeyCode.E) && isUnlocked &&
            GameObject.FindWithTag("OverworldManager").GetComponent<OverWorldManager>()._overworldIsActive) {
            isOpened = true;
            GameObject.FindWithTag("PlayerCharacter").GetComponent<CharacterBehavior>().ActivateChest();

            int randLootIndex = UnityEngine.Random.Range(0, numLootItems);
            Instantiate(possibleLoot[randLootIndex], transform.position, Quaternion.identity);
        }
    }

    private void SetSprite() {
        if (!isOpened) {
            spriteRenderer.sprite = chamber.isChestUnlocked ? chestUnlockedSprite : chestLockedSprite;
        } else {
            spriteRenderer.sprite = chestOpenedSprite;
        }
    }
}

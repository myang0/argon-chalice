using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxItems = 8;

    public List<BaseItem> itemList = new List<BaseItem>();

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    void Update() {

    }

    public bool AddItem(BaseItem itemToBeAdded) {
        if (itemList.Count >= maxItems) return false;

        itemList.Add(itemToBeAdded);
        Debug.Log(itemList.Count);
        return true;
    }

    public bool UseItemWithIndex(int index) {
        if (index < 0 || index >= itemList.Count) return false;

        BaseItem item = itemList[index];
        item.Use();

        itemList.Remove(item);
        Debug.Log(itemList.Count);
        Destroy(item.gameObject);
        return true;
    }

    public bool RemoveItemWithIndex(int index) {
        if (index < 0 || index >= itemList.Count) return false;

        itemList.Remove(itemList[index]);
        return true;
    }
}

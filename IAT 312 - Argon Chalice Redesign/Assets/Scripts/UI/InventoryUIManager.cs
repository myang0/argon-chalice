using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    private Inventory inventory;

    [SerializeField] private Text[] buttonTexts;
    [SerializeField] private GameObject eText;
    
    private UIManager uiManager;
    private BattleSystem battleSys;

    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();

        battleSys = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        SetText();
    }

    public void UseItem(int index) {
        if (!inventory.UseItemWithIndex(index)) return;

        battleSys.StartEnemyPhaseOnDelay();
        uiManager.DisableButtons();

        eText.SetActive(true);
        SetText();

        gameObject.SetActive(false);
    }

    private void SetText() {
        List<BaseItem> iList = inventory.itemList;

        for (int i = 0; i < buttonTexts.Length; i++) {
            buttonTexts[i].text = (i < iList.Count) ? iList[i].GetName() : "";
        }
    }

    IEnumerator EnemyPhaseTransition() {
        yield return new WaitForSeconds(1.5f);

        battleSys.StartEnemyPhase();
    }
}

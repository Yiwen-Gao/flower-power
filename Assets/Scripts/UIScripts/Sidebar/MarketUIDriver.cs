using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketUIDriver : MonoBehaviour
{
    private Dictionary<string, ItemData> items;
    public GameObject market;
    public GameObject marketObjPrefab;

    public void Start() {
        items = new Dictionary<string, ItemData>();
        foreach (Object item in Resources.LoadAll("Items", typeof(ItemData))) {
            items.Add(item.name, (ItemData) item);
            DisplayItem((ItemData) item);
        }
    }

    private void DisplayItem(ItemData item) {
        GameObject itemContainer = Instantiate(marketObjPrefab);
        itemContainer.transform.SetParent(market.transform);

        MarketUICell mcell = itemContainer.GetComponent<MarketUICell>();
        mcell.GetComponent<RectTransform>().localScale = Vector3.one;
        mcell.UpdateStats(item);
        mcell.buyButton.onClick.AddListener(() => {
            BuyItem(item.name);
        });
    }

    public void BuyItem(string name) {
        Player p = PlayerManager.Instance.currPlayer;
        if (items.ContainsKey(name)) {
            ItemData item = items[name];
            if (p.getFlowerCount() >= item.cost) {
                p.PayWithFlowers(item.name, item.cost);
            }
        }
    }
}

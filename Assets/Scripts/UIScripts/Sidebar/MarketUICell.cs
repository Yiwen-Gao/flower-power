using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketUICell : MonoBehaviour {
    public Image image;
    public Button buyButton;
    public new Text name;
    public Text cost;

    public void UpdateStats(ItemData item) {
        image.sprite = item.image;
        name.text = item.display_name;
        cost.text = item.cost.ToString();
    }
}

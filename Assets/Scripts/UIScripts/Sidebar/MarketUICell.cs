using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
// using UnityEngine.EventSystems;

public class MarketUICell : MonoBehaviour {
    public GameObject imageContainer;
    public Button buyButton;
    public new Text name;
    public Text cost;

    public void UpdateStats(ItemData item) {
        imageContainer.GetComponent<Image>().sprite = item.image;
        name.text = item.display_name;
        cost.text = item.cost.ToString();
    }
}

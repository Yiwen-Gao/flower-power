using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName = "Item Stats", order = 1)]
public class ItemData : ScriptableObject
{
    // inherited field `name` is filename used for paths
    // new field `display_name` is formatted with spacing used for the UI
    // e.g. `name`: "TrumpetFlower", `display_name`: "Trumpet Flower"
    public string display_name;
    public int cost;
    public Sprite image;
}
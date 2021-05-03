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

    public static ItemData CreateInstance(ItemData original) {
        ItemData copy = ScriptableObject.CreateInstance<ItemData>();
        
        copy.name = original.name;
        copy.display_name = original.display_name;
        copy.cost = original.cost;
        copy.image = original.image;

        return copy;
    }
}
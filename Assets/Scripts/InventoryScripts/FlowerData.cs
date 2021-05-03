using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Flower", menuName = "Flower Stats", order = 1)]
public class FlowerData : ScriptableObject
{
    // inherited field `name` is filename used for paths
    // new field `display_name` is formatted with spacing used for the UI
    // e.g. `name`: "TrumpetFlower", `display_name`: "Trumpet Flower"
    public string display_name;
    public Sprite image;
    public int time_to_harvest;
    public int abundance;

    public static FlowerData CreateInstance(FlowerData original) {
        FlowerData copy = ScriptableObject.CreateInstance<FlowerData>();

        copy.name = original.name;
        copy.display_name = original.display_name;
        copy.image = original.image;
        copy.time_to_harvest = original.time_to_harvest;
        copy.abundance = original.abundance;

        return copy;
    }
}
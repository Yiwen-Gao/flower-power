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
}
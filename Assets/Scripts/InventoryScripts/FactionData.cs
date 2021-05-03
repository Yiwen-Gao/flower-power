using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Faction", menuName = "Faction Stats", order = 1)]
public class FactionData : ScriptableObject
{
    public FlowerData flower_data;
    public Sprite icon;
    public Color color;

    public static FactionData CreateInstance(FactionData original) {
        FactionData copy = ScriptableObject.CreateInstance<FactionData>();
        
        copy.name = original.name;
        copy.flower_data = original.flower_data;
        copy.icon = original.icon;
        copy.color = original.color;

        return copy;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Faction", menuName = "Faction Stats", order = 1)]
public class FactionData : ScriptableObject
{
    public FlowerData flower_data;
    public Sprite icon;
    public Color color;
}
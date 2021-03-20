using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Flower", menuName = "Flower Stats", order = 1)]
public class FlowerData : ScriptableObject
{
    public string flower_name;
    public Texture image;
}
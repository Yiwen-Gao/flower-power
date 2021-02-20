using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public int x_coord { get; private set; }
    public int y_coord { get; private set; }

    public void setCoords(int x, int y)
    {
        x_coord = x;
        y_coord = y;
    }
}

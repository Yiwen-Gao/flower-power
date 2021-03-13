using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string,int> items;

    public void Start() 
    { 
        items = new Dictionary<string,int>();
    }

    public bool AddItem(string name, int count)
    {
        if (items.ContainsKey(name))
        {
            items[name] +=  count;
        }
        else 
        {
            items.Add(name,count);
        }
        return true;
    }

    public bool RemoveItem(string name, int count)
    {
        if (items.ContainsKey(name) && items[name] >= count)
        {
            items[name] -= count;
            return true;
        }
        return false;
    }



}
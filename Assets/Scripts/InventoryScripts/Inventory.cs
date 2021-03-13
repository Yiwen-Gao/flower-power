using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string,int> items;

    public Start() 
    { 
        items = new Dictionary<string,int>();
    }

    public boolean AddItem(string name, int count)
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

    public boolean RemoveItem(string name, int count)
    {
        if (items.ContainsKey(name) && items[name] >= count)
        {
            items[name] -= count;
            return true;
        }
        return false;
    }



}
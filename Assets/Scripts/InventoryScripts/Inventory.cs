using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string,int> allItems;

    private Dictionary<string,int> tradeItems;

    public void Start() 
    { 
        allItems = new Dictionary<string,int>();
        tradeItems = new Dictionary<string,int>();

        /* AddItem(allItems, "TrumpetFlower", 1);
        AddItem(allItems, "Lavender", 2);
        for (char c = 'a'; c <= 'z'; c++)
        {
            AddItem(allItems, c.ToString(), 1);
        } */
    }

    private bool AddItem(Dictionary<string,int> items, string name, int count)
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

    private bool RemoveItem(Dictionary<string,int> items, string name, int count)
    {
        if (items.ContainsKey(name) && items[name] >= count)
        {
            items[name] -= count;
            return true;
        }
        return false;
    }

    public void AddToTradeItems(string name, int count)
    {
        AddItem(tradeItems, name, count);
    }

    public void RemoveFromTradeItems(string name, int count)
    {
        RemoveItem(tradeItems, name, count);
    }

    public void confirmTrade(Dictionary<string,int> receivedItems) {
        foreach(var item in tradeItems) 
        {
            allItems[item.Key] -= item.Value;
        }
        foreach(var item in receivedItems) 
        {
            if (allItems.ContainsKey(item.Key))
            {
                allItems[item.Key] += item.Value;
            }
            else 
            {
                allItems.Add(item.Key,item.Value);
            }
        }
    }

    public Dictionary<string,int> getTradeItems() {
        return tradeItems;
    }

    public void emptyTradeItems() {
        tradeItems = new Dictionary<string,int>();
    }




}
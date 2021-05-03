using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    private Dictionary<string,int> allItems;
    private Dictionary<string,int> tradeItems;

    public void Awake() 
    {
        allItems = new Dictionary<string,int>();
        tradeItems = new Dictionary<string,int>();
    }

    private bool AddItem(Dictionary<string,int> items, string name, int count)
    {
        if (items.ContainsKey(name))
        {
            items[name] += count;
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

    public bool AddToTradeItems(string name, int count)
    {
        int prevCount = tradeItems.ContainsKey(name) ? tradeItems[name] : 0;
        if (!allItems.ContainsKey(name) || (allItems[name] < count + prevCount)) 
        {
            return false;
        }
        return AddItem(tradeItems, name, count);
    }

    public bool RemoveFromTradeItems(string name, int count)
    {
        return RemoveItem(tradeItems, name, count);
    }

    public bool AddToAllItems(string name, int count)
    {
        bool isValid = AddItem(allItems, name, count);
        PlayerManager.Instance.UpdateInventoryUI();
        return isValid;
    }
    
    public bool RemoveFromAllItems(string name, int count)
    {
        bool isValid = RemoveItem(allItems, name, count);
        PlayerManager.Instance.UpdateInventoryUI();
        return isValid;
    }

    public void ConfirmTrade(Dictionary<string,int> receivedItems) {
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

    private int GetItemCount(Dictionary<string, int> items, string name) {
        if (!items.ContainsKey(name)) return 0;
        return items[name];
    }

    public int GetAllItemCount(string name) {
        return GetItemCount(allItems, name);
    }

    public int GetTradeItemCount(string name) {
        return GetItemCount(tradeItems, name);
    }

    public Dictionary<string,int> GetTradeItems() {
        return tradeItems;
    }

    public Dictionary<string,int> GetAllItems() {
        return allItems;
    }
    
    public void ResetTradeItems() {
        tradeItems = new Dictionary<string,int>();
    }
}
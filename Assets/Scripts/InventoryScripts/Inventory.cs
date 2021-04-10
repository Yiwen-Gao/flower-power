using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string,int> allItems;
    private Dictionary<string,int> tradeItems;

    public void Awake() 
    {
        allItems = new Dictionary<string,int>();
        tradeItems = new Dictionary<string,int>();
    }

    public void Start() 
    { 
        //AddToAllItems("TrumpetFlower", Random.Range(1,30));
        //AddToAllItems("Lavender", Random.Range(1,30));
        
        /*for (char c = 'a'; c <= 'z'; c++)
        {
            AddToAllItems(c.ToString(), 1);
        }*/
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
        PlayerManager.Instance.UpdateInventoryUI();
        return true;
    }

    private bool RemoveItem(Dictionary<string,int> items, string name, int count)
    {
        if (items.ContainsKey(name) && items[name] >= count)
        {
            items[name] -= count;
            PlayerManager.Instance.UpdateInventoryUI();
            return true;
        }
        PlayerManager.Instance.UpdateInventoryUI();
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
        return AddItem(allItems, name, count);
    }
    
    public bool RemoveFromAllItems(string name, int count)
    {
        return RemoveItem(allItems, name, count);
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
        PlayerManager.Instance.UpdateInventoryUI();
    }

    public Dictionary<string,int> GetTradeItems() {
        return tradeItems;
    }

    public Dictionary<string,int> GetAllItems() {
        return allItems;
    }

    public void emptyTradeItems() {
        tradeItems = new Dictionary<string,int>();
    }

    public void ResetTradeItems() {
        tradeItems = new Dictionary<string,int>();
    }
}
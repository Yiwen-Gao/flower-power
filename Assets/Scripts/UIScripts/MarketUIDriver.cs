using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketUIDriver : MonoBehaviour
{
    public void BuyBeehive() {
        Player p = PlayerManager.Instance.currPlayer;
        if (p.inventory.GetItemCount("TrumpetFlower") > 1 && p.inventory.GetItemCount("Lavender") > 1) {
            p.inventory.RemoveFromAllItems("TrumpetFlower", 1);
            p.inventory.RemoveFromAllItems("Lavender", 1);
            p.inventory.AddToAllItems("Beehive", 1);
            
            TradeUIDriver.Instance.UpdateTradePlayers(p);
        }
    }
    
    public void BuyHerbicide() {
        Player p = PlayerManager.Instance.currPlayer;
        if (p.inventory.GetItemCount("TrumpetFlower") > 1 && p.inventory.GetItemCount("Lavender") > 1) {
            p.inventory.RemoveFromAllItems("TrumpetFlower", 1);
            p.inventory.RemoveFromAllItems("Lavender", 1);
            p.inventory.AddToAllItems("Herbicide", 1);
            
            TradeUIDriver.Instance.UpdateTradePlayers(p);
        }
    }
}

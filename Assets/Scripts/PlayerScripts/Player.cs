using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string player_name;
    public int player_number;

    public FactionData player_faction;
    
    public List<Hex> owned_hexes;
    public List<Hex> candidate_hexes; //hexes we might be able to expand to this turn

    public Inventory inventory; 
    public int initial_flower_amount;

    public string selected_item;

    public void InitializeInventory() {
        AddToAllItems(player_faction.flower_data.name, initial_flower_amount);
    }

    public void ClaimHex(Hex to_claim) //should we check for whether this is valid here?
    {
        //Debug.Log(player_number + " claimed hex(" + to_claim.x_coord + " " + to_claim.y_coord + ")");
        to_claim.owner = this;
        owned_hexes.Add(to_claim);

        List<Hex> neighbors = HexGrid.Instance.hexes_within_distance(to_claim, 1);
        foreach (Hex h in neighbors)
        {
            candidate_hexes.Add(h);
        }
        
        PlayerDisplayManager.Instance.AddClaimHighlight(to_claim);
        UpdateDisplay();
    }

    public void UpdateDisplay() 
    {
        // display player's name and color
        PlayerDisplayManager.Instance.SetCurrentPlayerInfo(this.player_name, this.player_faction);

        // checks candidates and display player's territory
        PlayerDisplayManager.Instance.ClearTemporary();
        CheckCandidates();
        PlayerDisplayManager.Instance.BuildTemporary(candidate_hexes);

        // display player's items
        InvUIDriver.Instance.UpdateInventory(this);
        // TradeUIDriver.Instance.UpdateTradePlayers(this);
    }

    public void RemoveHighlights()
    {
        foreach (Hex h in candidate_hexes)
        {
            h.GetComponent<SpriteRenderer>().color = Color.white;
        }
        
        foreach (Hex h in owned_hexes)
        {
            h.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void CheckCandidates()
    {
        List<Hex> to_remove = new List<Hex>();
        foreach (Hex h in candidate_hexes)
        {
            if (h.owner != null)
            {
                to_remove.Add(h);
            }
        }

        foreach (Hex h in to_remove)
        {
            candidate_hexes.Remove(h);
        }
    }

    public int getFlowerCount() {
        string factionFlower = player_faction.flower_data.name;
        if (inventory.allItems.ContainsKey(factionFlower)) {
            return inventory.allItems[factionFlower];
        } 

        return 0;
    }

    public void PayWithFlowers(string itemName, int itemCost) {
        string factionFlower = player_faction.flower_data.name;
        inventory.RemoveFromAllItems(factionFlower, itemCost);
        inventory.AddToAllItems(itemName, 1);
    }

    public void AddToTradeItems(string item, int count) {
        inventory.AddToTradeItems(item, count);
    }

    public void RemoveFromTradeItems(string item, int count) {
        inventory.RemoveFromTradeItems(item, count);
    }
    
    public void AddToAllItems(string item, int count) {
        inventory.AddToAllItems(item, count);
    }

    public void RemoveFromAllItems(string item, int count) {
        inventory.RemoveFromAllItems(item, count);
    }

    public void Plant(Hex h) {
        string selected_item = "Lavendar";
        h.AddItemToHex(selected_item);
        inventory.RemoveFromAllItems(selected_item, 1);
        // UpdateDisplay();
    }

    public void UpdatePlantTimes() {
        foreach (Hex h in owned_hexes) {
            if (h.plant_name != null) {
                h.plant_time += 1;
            }
        }
    }

    // private void Harvest(Hex h) {
    //     inventory.AddToAllItems(h.plant_name, h.plant_abundance);
    //     h.plant_time = 0;
    // }
}

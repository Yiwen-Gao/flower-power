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

    private int remainingExpansions;

    public void Start() {
        remainingExpansions = PlayerManager.Instance.expansionsPerTurn;
    }

    public void InitializeInventory() {
        inventory.AddToAllItems(player_faction.flower_data.name, initial_flower_amount);
    }

    public void ClaimHex(Hex to_claim) //should we check for whether this is valid here?
    {
        if (remainingExpansions > 0) {
            to_claim.owner = this;
            owned_hexes.Add(to_claim);

            List<Hex> neighbors = HexGrid.Instance.hexes_within_distance(to_claim, 1);
            foreach (Hex h in neighbors)
            {
                candidate_hexes.Add(h);
            }
            
            PlayerDisplayManager.Instance.AddClaimHighlight(to_claim);
            UpdateDisplay();
            remainingExpansions--;
        }
    }

    public void UpdateData() {
        // reset number of hexes that player can claim in turn
        remainingExpansions = PlayerManager.Instance.expansionsPerTurn;
        // display player's name and color
        PlayerDisplayManager.Instance.SetCurrentPlayerInfo(this.player_name, this.player_faction);

        UpdateDisplay();
        UpdateInventory();
        UpdatePlantTime();
    }

    // checks candidates and display player's territory
    private void UpdateDisplay() 
    {
        PlayerDisplayManager.Instance.ClearTemporary();
        CheckCandidates();
        PlayerDisplayManager.Instance.BuildTemporary(candidate_hexes);
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

    // update display of player's items
    public void UpdateInventory() {
        InvUIDriver.Instance.UpdateInventory(this);
    }

    public int getFlowerCount() {
        string factionFlower = player_faction.flower_data.name;
        return inventory.GetAllItemCount(factionFlower);
    }

    public void PayWithFlowers(string itemName, int itemCost) {
        string factionFlower = player_faction.flower_data.name;
        inventory.RemoveFromAllItems(factionFlower, itemCost);
        inventory.AddToAllItems(itemName, 1);
    }

    private void UpdatePlantTime() {
        foreach (Hex h in owned_hexes) {
            h.UpdatePlantTime();
        }
    }

    public void Plant(Hex h) {
        if (inventory.GetAllItemCount(selected_item) > 0) {
            bool isValid = h.AddItem(selected_item);
            if (isValid) {
                inventory.RemoveFromAllItems(selected_item, 1);
            }
        }
    }

    public void Harvest(Hex h) {
        FlowerData data = h.Harvest();
        if (data) {
            inventory.AddToAllItems(data.name, data.abundance);
        }
    }
}

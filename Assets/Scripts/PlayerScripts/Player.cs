using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string player_name;
    public int player_number;
    
    public List<Hex> owned_hexes;
    public List<Hex> candidate_hexes; //hexes we might be able to expand to this turn

    /*public List<Flower> flowers;
    public List<Seed> seeds;*/

    public void ClaimHex(Hex to_claim) //should we check for whether this is valid here?
    {
        Debug.Log(player_number + " claimed hex(" + to_claim.x_coord + " " + to_claim.y_coord + ")");
        to_claim.owner = this;
        owned_hexes.Add(to_claim);

        List<Hex> neighbors = HexGrid.Instance.hexes_within_distance(to_claim, 1);
        foreach (Hex h in neighbors)
        {
            candidate_hexes.Add(h);
        }
        
        UpdateDisplay();
    }

    public void UpdateDisplay() // checks candidates and sets up highlighting
    {
        RemoveHighlights();
        CheckCandidates();
        DisplayCandidates();
        DisplayClaimed();
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

    private void DisplayCandidates()
    {
        if (this != PlayerManager.Instance.currPlayer) 
        {
            return;
        }
        
        foreach (Hex h in candidate_hexes)
        {
            h.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void DisplayClaimed()
    {
        foreach (Hex h in owned_hexes)
        {
            h.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    
    //DEBUG CODE

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10,10,400,700));

        if (GUILayout.Button("Clear highlights"))
        {
            RemoveHighlights();
        }
        
        if (GUILayout.Button("Add highlights"))
        {
            UpdateDisplay();
        }
        
        GUILayout.EndArea();
    }
}

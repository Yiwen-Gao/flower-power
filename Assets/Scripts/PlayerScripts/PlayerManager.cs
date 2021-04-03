using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayerManager>();
             
                if (_instance == null)
                {
                    return null;
                }
            }
     
            return _instance;
        }
    }

    public List<Player> players = new List<Player>();
    private int idx;
    public Player currPlayer;

    public GameObject mainCamera;

    // Start is called before the first frame update
    void Start() {
        idx = 0; //players.Count-1;
        currPlayer = players[idx];
        AssignInitialTerritory();

        GetNextPlayer();
    }

    private void AssignInitialTerritory() {
        List<Hex> availableLand = new List<Hex>();
        Hex[,] hexgrid = HexGrid.Instance.hexgrid;
        for (int i = 0; i < HexGrid.Instance.width; i++) {
            for (int j = 0; j < HexGrid.Instance.height; j++) {
                if (hexgrid[i, j].terrain != TileType.Water) {
                    availableLand.Add(hexgrid[i, j]);
                }
            }
        }

        foreach (Player player in players) {
            int rand = Random.Range(0, availableLand.Count);
            Hex hex = availableLand[rand];
            player.ClaimHex(hex);
            availableLand.RemoveAt(rand);
        }
    }

    private Vector2 FindCenter(List<Hex> territory) {
        float left, right, top, bottom;
        left = right = top = bottom = 0f;
        if (territory.Count != 0)
        {
            left = right = territory[0].transform.position.x;
            top = bottom = territory[0].transform.position.y;
        }
        foreach (Hex hex in territory) {
            left = Mathf.Min(left, hex.transform.position.x);
            right = Mathf.Max(right, hex.transform.position.x);
            top = Mathf.Max(top, hex.transform.position.y);
            bottom = Mathf.Min(bottom, hex.transform.position.y);
        }

        return new Vector2((left + right) / 2, (top + bottom) / 2);
    }

    private void MoveCamera(Player player) {
        List<Hex> territory = player.owned_hexes;
        Vector2 center = FindCenter(territory);
        float currHeight = mainCamera.transform.position.z;
        CameraScript.Instance.SetTarget(new Vector3(center.x, center.y, currHeight));
        CameraScript.Instance.ResetZoom();
    }

    public void GetNextPlayer() {
        currPlayer = players[(idx++) % players.Count];
        MoveCamera(currPlayer);
        currPlayer.UpdateDisplay();
        updatePlantTime(currPlayer);
    }

    public void Trade(Player otherPlayer) {
        Dictionary<string,int> currItems = currPlayer.inventory.GetTradeItems();
        Dictionary<string,int> otherItems = otherPlayer.inventory.GetTradeItems();
        
        currPlayer.inventory.ConfirmTrade(otherItems);
        otherPlayer.inventory.ConfirmTrade(currItems);
        
        currPlayer.inventory.ResetTradeItems();
        otherPlayer.inventory.ResetTradeItems();
    }

    public void updatePlantTime(Player currPlayer) {
        foreach (Hex h in currPlayer.owned_hexes) {
            if (h.plant_name != null) {
                h.plant_time += 1;
            }
        }
    }

    public void UpdateInventoryUI()
    {
        InvUIDriver.Instance.UpdateInventory(currPlayer.inventory);
    }

    // public void harvestPlants() {
    //     foreach (Hex h in currPlayer.owned_hexes) {
    //         // if ( h.plant_time == time_to_harvest) {
    //         //     currPlayer.harvest(h);
    //         // }
    // }
}

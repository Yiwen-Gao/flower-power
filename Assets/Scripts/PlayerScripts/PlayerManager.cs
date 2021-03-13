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
        idx = players.Count-1;
        currPlayer = players[idx];
        AssignInitialTerritory();

        GetNextPlayer();
    }

    void AssignInitialTerritory() {
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

    Vector2 FindCenter(List<Hex> territory) {
        float left, right, top, bottom;
        left = right = top = bottom = 0f;
        foreach (Hex hex in territory) {
            left = Mathf.Min(left, hex.x_coord);
            right = Mathf.Max(right, hex.x_coord);
            top = Mathf.Min(left, hex.y_coord);
            bottom = Mathf.Max(right, hex.y_coord);
        }

        return new Vector2(
            (left + right) / 2,
            (top + bottom) / 2
        );
    }

    public void MoveCamera(Player player) {
        List<Hex> territory = player.owned_hexes;
        Vector2 center = FindCenter(territory);
        float cameraHeight = mainCamera.transform.position.z;
        mainCamera.transform.position = new Vector3(center.x, center.y, cameraHeight);
    }

    public void GetNextPlayer() {
        currPlayer = players[(idx++) % players.Count];
        MoveCamera(currPlayer);
        currPlayer.UpdateDisplay();
        // return currPlayer;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 100,Screen.height - 50,100,50));

        if (GUILayout.Button("Next Player"))
        {
            GetNextPlayer();
        }

        GUILayout.EndArea();
    }
}

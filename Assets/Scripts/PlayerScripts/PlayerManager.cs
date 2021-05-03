using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private Object[] factions;
    public List<Player> players = new List<Player>();
    private int idx;
    public Player currPlayer;
    public GameObject playerPrefab;

    public readonly int MIN_PLAYER_NUM = 2;
    public readonly int MAX_PLAYER_NUM = 6;

    public int expansionsPerTurn = 10;

    void Start() {
        DontDestroyOnLoad(PlayerManager.Instance);
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.buildIndex == 1) {
            OnGameSceneLoaded(activeScene, LoadSceneMode.Additive);
        } else {
            SceneManager.sceneLoaded += PlayerManager.Instance.OnGameSceneLoaded;
        }

        factions = Resources.LoadAll("Factions", typeof(FactionData));
    }

    public void OnGameSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 2) {
            idx = 0; //players.Count-1;
            currPlayer = players[idx];
            AssignInitialTerritory();
            GetNextPlayer();
        }
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
        float currHeight = CameraScript.Instance.transform.position.z;
        CameraScript.Instance.SetTarget(new Vector3(center.x, center.y, currHeight));
        CameraScript.Instance.ResetZoom();
    }

    public void GetNextPlayer() {
        currPlayer = players[(idx++) % players.Count];
        currPlayer.UpdateData();
        MoveCamera(currPlayer);
    }

    public void AddNewPlayer(string name) {
        GameObject gameObject = Instantiate(playerPrefab, transform);
        Player player = gameObject.GetComponent<Player>();
        player.player_name = name;
        player.player_number = players.Count;
        player.player_faction = (FactionData) factions[players.Count % factions.Length];
        player.InitializeInventory();

        players.Add(player);
    }

    public void UpdateInventoryUI() {
        if (currPlayer == null) return;
        currPlayer.UpdateInventory();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public List<Player> players = new ArrayList<>();
    private int idx;
    public Player currPlayer;

    public HexGrid grid;
    public GameObject camera;

    // Start is called before the first frame update
    void Start() {
        idx = 0;
        currPlayer = players[idx];
        AssignInitialTerritory();
    }

    void AssignInitialTerritory() {
        List<Hex> availableLand = new ArrayList<>();
        Hex[,] hexgrid = grid.hexgrid;
        for (int i = 0; i < hexgrid.Count; i++) {
            for (int j = 0; j < hexgrid[i].Count; j++) {
                if (hexgrid[i, j].terrain != TileType.Water) {
                    availableLand.Add(hexgrid[i, j]);
                }
            }
        }

        foreach (Player player in players) {
            int rand = Random.Range(0, availableLand.Count);
            Hex hex = availableLand[rand];
            player.AddTerritory(hex);
            availableLand.RemoveAt(rand);
        }
    }

    Vector2 FindCenter(List territory) {
        float left, right, top, bottom = 0f;
        foreach (Hex hex in territory) {
            left = Math.min(left, hex.x_coord);
            right = Math.max(right, hex.x_coord);
            top = Math.min(left, hex.y_coord);
            bottom = Math.max(right, hex.y_coord);
        }

        return new Vector2(
            (left + right) / 2,
            (top + bottom) / 2
        );
    }

    void MoveCamera(Player player) {
        List territory = player.GetTerritory();
        Vector2 center = player.FindCenter(territory);
        float cameraHeight = camera.transform.position.z;
        camera.transform.position = new Vector3(center.x, center.y, cameraHeight);
    }

    Player GetNextPlayer() {
        currPlayer = players[(idx++) % players.Count];
        MoveCamera(currPlayer);
        return currPlayer;
    }
}

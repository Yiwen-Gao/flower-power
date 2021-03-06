using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public int x_coord { get; private set; }
    public int y_coord { get; private set; }

    public TileType terrain;

    public void setCoords(Vector2Int coords)
    {
        x_coord = coords.x;
        y_coord = coords.y;
    }

    public void setTerrain(TileType t)
    {
        this.terrain = t;
        
        //randomize the sprite used for the tile
        int sublist_size = SpriteMaster.Instance.tile_sprites[(int) t].sprites.Count;

        int chosen_ind = 0;
        float sum = 0f;
        float random_num = Random.value;
        while (true)
        {
            if (chosen_ind >= sublist_size)
            {
                chosen_ind = 0;
                break;
            }
            sum += SpriteMaster.Instance.tile_sprites[(int) t].probabilities[chosen_ind];
            if (sum >= random_num) break;
            chosen_ind++;
        }
        this.GetComponent<SpriteRenderer>().sprite =
            SpriteMaster.Instance.tile_sprites[(int) t].sprites[chosen_ind];
    }

    /*public void OnMouseDown()
    { //debug neighbors
        Debug.Log("clicked");
        HexGrid parent_grid = transform.parent.GetComponent<HexGrid>();
        foreach (Vector2Int v in parent_grid.adjacent_coords(new Vector2Int(this.x_coord,this.y_coord)))
        {
            parent_grid.hexgrid[v.x,v.y].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }*/
}

public enum TileType
{
    Water = 0,
    Meadow = 1,
    Mountain = 2,
}
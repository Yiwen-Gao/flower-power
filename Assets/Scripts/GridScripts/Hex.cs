using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hex : MonoBehaviour
{
    public int x_coord { get; private set; }
    public int y_coord { get; private set; }

    public TileType terrain;

    public Player owner;

    public string plant_name;
    public int plant_abundance;
    public int plant_time; // so we know when to harvest 

    public GameObject flower_object;

    public void AddFlowerToHex(string obj_id)
    {
        FlowerData data = Resources.Load("Flowers/"+obj_id) as FlowerData;
        GameObject new_flower = Instantiate(flower_object, this.transform.position, Quaternion.identity, this.transform);
        new_flower.GetComponent<InvUICell>().UpdateStats(data.flower_name,1);
        new_flower.GetComponent<SpriteRenderer>().sprite = data.image;
        new_flower.GetComponent<SpriteRenderer>().sortingOrder = 20;

        this.plant_name = obj_id;
        this.plant_abundance = 1; // abundance varies for each hex/flower pair
        this.plant_time = 0; 
    }

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

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        //Debug.Log("clicked");
        Player p = PlayerManager.Instance.currPlayer;
        if (p.owned_hexes.Contains(this))
            p.Plant(this,"Lavender"); // test
        if (p.candidate_hexes.Contains(this))
            p.ClaimHex(this); //replace with current player
        
    }

    /*public void OnMouseEnter()
    {
        this.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public void OnMouseExit()
    {
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }*/
}

public enum TileType
{
    Water = 0,
    Meadow = 1,
    Mountain = 2,
}
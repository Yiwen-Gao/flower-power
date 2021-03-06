﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hex : MonoBehaviour
{
    public int x_coord { get; private set; }
    public int y_coord { get; private set; }

    public TileType terrain;

    public Player owner;
    private ItemData item;
    private FlowerData flower;

    private bool readyToHarvest;

    public GameObject containerPrefab;

    public GameObject itemContainer;
    public GameObject flowerContainer;
    public GameObject harvestSignalContainer;

    public void Start() {
        readyToHarvest = false;
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

        Player p = PlayerManager.Instance.currPlayer;
        if (p.owned_hexes.Contains(this)) {
            if (flower != null && flower.time_to_harvest == 0) {
                p.Harvest(this);
            } else {
                p.Plant(this); 
            }
        } else if (p.candidate_hexes.Contains(this)) {
            p.ClaimHex(this); //replace with current player
            GameObject effect = Instantiate(HexGrid.Instance.effect_object, transform.position, Quaternion.identity);
            effect.GetComponent<SpriteRenderer>().color = p.player_faction.color;
        } else if (p.selected_item == MarketItem.Weed && p.inventory.GetAllItemCount(p.selected_item) > 0) {
            bool isValid = AddItem(p.selected_item);
            if (isValid) {
                p.inventory.RemoveFromAllItems(p.selected_item, 1);
            }
        }
    }

    public bool AddItem(string obj_id) {
        Object obj = Resources.Load("Items/" + obj_id);
        if (readyToHarvest) {
            return false;
        } else if (flower != null && obj as ItemData) {
            ItemData newItem = ItemData.CreateInstance(obj as ItemData);
            return AddMarketItem(newItem);
        } else if (flower == null && obj as FlowerData) {
            flower = FlowerData.CreateInstance(obj as FlowerData);
            flowerContainer = DisplayItem(flowerContainer, flower.image);
            return true;
        } else {
            return false;
        }
    }

    private bool AddMarketItem(ItemData newItem) {
        bool isValid;

        switch (newItem.name) {
            case MarketItem.Beehive:
                if (item == null || item.name != MarketItem.Weed) {
                    isValid = true;
                    flower.abundance++;
                    item = null;
                } else {
                    isValid = false;
                }
                break;
            case MarketItem.Herbicide:
                if (item != null && item.name == MarketItem.Weed) {
                    isValid = true;
                    item = null;
                } else {
                    isValid = false;
                }
                break;
            case MarketItem.Weed:
                if (item == null || item.name != MarketItem.Weed) {
                    isValid = true;
                    item = newItem;
                } else {
                    isValid = false;
                }
                break;
            default:
                isValid = false;
                break;
        }

        if (isValid) {
            itemContainer = DisplayItem(itemContainer, newItem.image, true);
        }
        return isValid;
    }

    private GameObject DisplayItem(GameObject container, Sprite image, bool isMarketItem=false) {
        if (container != null) {
            Destroy(container);
        }
        container = Instantiate(containerPrefab, this.transform.position, Quaternion.identity, this.transform);
        container.GetComponent<SpriteRenderer>().sprite = image;
        container.GetComponent<SpriteRenderer>().sortingOrder = isMarketItem ? 
            SortingOrder.MarketItem : 
            SortingOrder.FlowerItem;

        return container;
    }

    public void UpdatePlantTime() {
        if (flower != null) {
            if (item == null || item.name != MarketItem.Weed) {
                if (flower.time_to_harvest > 0) {
                    flower.time_to_harvest--;
                } else {
                    readyToHarvest = true;
                    DisplayReadyToHarvestSignal();
                }
            }
        }
    }

    private void DisplayReadyToHarvestSignal() {
        if (!readyToHarvest) return;
        if (harvestSignalContainer == null) {
            harvestSignalContainer = Instantiate(
                containerPrefab, 
                this.transform.position, 
                Quaternion.identity, 
                this.transform
            );
        }
        GameObject textContainer = harvestSignalContainer.transform.GetChild(0).gameObject; 
        textContainer.GetComponent<Renderer>().sortingOrder = SortingOrder.HarvestSignal;
        textContainer.GetComponent<TextMesh>().text = $"{flower.abundance}x";
    }

    public FlowerData Harvest() {
        FlowerData data = flower;
        flower = null;
        item = null;
        readyToHarvest = false;

        Destroy(flowerContainer);
        Destroy(itemContainer);
        Destroy(harvestSignalContainer);

        return data;
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

public static class MarketItem {
    public const string Beehive = "Beehive";
    public const string Herbicide = "Herbicide";
    public const string Weed = "Weed";
}

public static class SortingOrder {
    public const int FlowerItem = 1;
    public const int MarketItem = 2;
    public const int HarvestSignal = 3;
}

public enum TileType
{
    Water = 0,
    Meadow = 1,
    Mountain = 2,
}
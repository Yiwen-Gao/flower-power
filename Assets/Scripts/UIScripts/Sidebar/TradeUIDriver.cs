using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TradeUIDriver : MonoBehaviour
{
    public static TradeUIDriver _instance;
    public static TradeUIDriver Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TradeUIDriver>();
             
                if (_instance == null)
                {
                    return null;
                }
            }
     
            return _instance;
        }
    }
    
    private List<GameObject> inventory_objects = new List<GameObject>();

    public GameObject grid_container;

    public GameObject trade_obj_prefab;

    public Dropdown select_player;

    public bool show_own_inventory;

    public Text player_name;
    // Start is called before the first frame update


    public List<Player> otherPlayerList;

    public Toggle toggle_box;

    public Image name_background;

    public void UpdateTradePlayers(Player player)
    {
        curr_player = player;
        toggle_box.isOn = true;
        SetCurrentContainer(true);
        
        player.inventory.ResetTradeItems();
        
        foreach (GameObject go in inventory_objects)
        {
            Destroy(go);
        }

        inventory_objects.Clear();

        player_name.text = player.player_name;
        name_background.color = player.player_faction.color;

        foreach (KeyValuePair<String, int> kvp in player.inventory.allItems)
        {
            if (kvp.Value == 0) continue;
            GameObject new_inv_object = Instantiate(trade_obj_prefab);
            TradeUICell tcell = new_inv_object.GetComponent<TradeUICell>();
            tcell.UpdateStats(kvp.Key,kvp.Value);
            tcell.linked_player = player;
            new_inv_object.transform.SetParent(grid_container.transform);
            tcell.GetComponent<RectTransform>().localScale = Vector3.one;
            inventory_objects.Add(new_inv_object);
        }
        
        select_player.ClearOptions();
        otherPlayerList.Clear();
        List<Dropdown.OptionData> new_options = new List<Dropdown.OptionData>();
        foreach (Player p in PlayerManager.Instance.players)
        {
            if (p == player) continue;
            Dropdown.OptionData new_option = new Dropdown.OptionData();
            new_option.text = p.player_name;
            new_options.Add(new_option);
            otherPlayerList.Add(p);
        }

        select_player.options = new_options;
        select_player.value = 0;
        UpdateOtherContainer(0);
    }

    public GameObject other_container;
    public GameObject self_container;
    public Image toggle_background;
    public void SetCurrentContainer(bool is_self)
    {
        self_container.SetActive(is_self);
        other_container.SetActive(!is_self);

        toggle_box.transform.rotation = (is_self) ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
        toggle_background.color = (is_self) ? curr_player.player_faction.color : other_player.player_faction.color;
    }
    
    
    private List<GameObject> other_inventory_objects = new List<GameObject>();

    public GameObject other_grid_container;

    private Player curr_player;
    private Player other_player;
    public void UpdateOtherContainer(int other)
    {
        other_player = otherPlayerList[other];
        other_player.inventory.ResetTradeItems();
        foreach (GameObject go in other_inventory_objects)
        {
            Destroy(go);
        }

        other_inventory_objects.Clear();

        ColorBlock temp = select_player.colors;
        temp.normalColor = other_player.player_faction.color;
        temp.highlightedColor = other_player.player_faction.color;
        temp.selectedColor = other_player.player_faction.color;
        select_player.colors = temp;

        foreach (KeyValuePair<String, int> kvp in other_player.inventory.allItems)
        {
            if (kvp.Value == 0) continue;
            GameObject new_inv_object = Instantiate(trade_obj_prefab);
            TradeUICell tcell = new_inv_object.GetComponent<TradeUICell>();
            tcell.UpdateStats(kvp.Key,kvp.Value);
            tcell.linked_player = other_player;
            new_inv_object.transform.SetParent(other_grid_container.transform);
            tcell.GetComponent<RectTransform>().localScale = Vector3.one;
            other_inventory_objects.Add(new_inv_object);
        }
        
        SetCurrentContainer(toggle_box.isOn);
    }

    public void Trade()
    {
        curr_player.inventory.ConfirmTrade(other_player.inventory.GetTradeItems());
        other_player.inventory.ConfirmTrade(curr_player.inventory.GetTradeItems());
        curr_player.inventory.ResetTradeItems();
        other_player.inventory.ResetTradeItems();
        UpdateTradePlayers(curr_player);
    }
}

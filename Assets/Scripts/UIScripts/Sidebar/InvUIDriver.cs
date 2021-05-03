using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvUIDriver : MonoBehaviour
{
    
    public static InvUIDriver _instance;
    public static InvUIDriver Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<InvUIDriver>();
             
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

    public GameObject inv_obj_prefab;

    public void UpdateInventory(Player player)
    {
        foreach (GameObject go in inventory_objects)
        {
            Destroy(go);
        }

        inventory_objects.Clear();

        foreach (KeyValuePair<String, int> kvp in player.inventory.GetAllItems())
        {
            if (kvp.Value == 0) continue;
            GameObject new_inv_object = Instantiate(inv_obj_prefab);
            new_inv_object.GetComponent<InvUICell>().UpdateStats(kvp.Key,kvp.Value);
            new_inv_object.transform.SetParent(grid_container.transform);
            new_inv_object.GetComponent<RectTransform>().localScale = Vector3.one;
            new_inv_object.GetComponent<Button>().onClick.AddListener(() => {
                Player currPlayer = PlayerManager.Instance.currPlayer;
                currPlayer.selected_item = kvp.Key;
            });
            inventory_objects.Add(new_inv_object);
        }

        // have to update trade container after buying items
        TradeUIDriver.Instance.UpdateTradePlayers(player);
    }
}

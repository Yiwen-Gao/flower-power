using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateInventory(Inventory playerInv)
    {
        foreach (GameObject go in inventory_objects)
        {
            Destroy(go);
        }

        inventory_objects.Clear();

        foreach (KeyValuePair<String, int> kvp in playerInv.allItems)
        {
            GameObject new_inv_object = Instantiate(inv_obj_prefab);
            new_inv_object.GetComponent<InvUICell>().UpdateStats(kvp.Key,kvp.Value);
            new_inv_object.transform.SetParent(grid_container.transform);
            inventory_objects.Add(new_inv_object);
        }
    }
}

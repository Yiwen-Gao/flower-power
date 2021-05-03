using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TradeUICell : MonoBehaviour
{

    public GameObject img_obj;

    public GameObject obj_name_obj;

    public GameObject obj_count_obj;

    private Image img;

    private new Text name;

    private Text count;

    public int marked_count;
    public int max_count;
    public Player linked_player;
    public String obj_id;

    void Awake()
    {
        img = img_obj.GetComponent<UnityEngine.UI.Image>();
        name = obj_name_obj.GetComponent<Text>();
        count = obj_count_obj.GetComponent<Text>();
        marked_count = 0;
    }

    public void UpdateStats(string obj_id, int obj_count, int curr_count) {
        UnityEngine.Object obj = Resources.Load("Items/" + obj_id);
        if (obj as ItemData) {
            ItemData data = obj as ItemData;
            SetStats(data.image, data.display_name);
        }
        else if (obj as FlowerData) {
            FlowerData data = obj as FlowerData;
            SetStats(data.image, data.display_name);
        }
        else {
            FlowerData data = Resources.Load("Items/Placeholder") as FlowerData;
            SetStats(data.image, obj_id);
        }

        count.text = curr_count + "/" + obj_count;
        marked_count = curr_count;
        max_count = obj_count;
        this.obj_id = obj_id;
    }

    private void SetStats(Sprite image, string name) {
        img.sprite = image;
        this.name.text = name;
    }

    public void AddTrade()
    {
        marked_count++;
        if (marked_count > max_count) marked_count = max_count;
        else
        {
            linked_player.inventory.AddToTradeItems(obj_id, 1);
        }

        count.text = marked_count + "/" + max_count;
    }

    public void SubtractTrade()
    {
        marked_count--;
        if (marked_count < 0) marked_count = 0;
        else
        {
            linked_player.inventory.RemoveFromTradeItems(obj_id, 0);
        }
        
        count.text = marked_count + "/" + max_count;
    }

}
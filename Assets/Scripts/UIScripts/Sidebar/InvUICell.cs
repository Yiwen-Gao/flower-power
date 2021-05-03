using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class InvUICell : MonoBehaviour
{

    public GameObject img_obj;

    public GameObject obj_name_obj;

    public GameObject obj_count_obj;

    private Image img;

    private new Text name;

    private Text count;

    void Awake()
    {
        img = img_obj.GetComponent<UnityEngine.UI.Image>();
        name = obj_name_obj.GetComponent<Text>();
        count = obj_count_obj.GetComponent<Text>();
    }

    public void UpdateStats(string obj_id, int obj_count) {
        Object obj = Resources.Load("Items/" + obj_id);
        if (obj as ItemData) {
            ItemData data = obj as ItemData;
            SetStats(data.image, data.display_name, obj_count);
        } else if (obj as FlowerData) {
            FlowerData data = obj as FlowerData;
            SetStats(data.image, data.display_name, obj_count);
        } else {
            FlowerData data = Resources.Load("Items/Placeholder") as FlowerData;
            SetStats(data.image, obj_id, obj_count);
        }
    }

    private void SetStats(Sprite image, string name, int count) {
        img.sprite = image;
        this.name.text = name;
        this.count.text = count.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvUICell : MonoBehaviour
{

    public GameObject img_obj;

    public GameObject obj_name_obj;

    public GameObject obj_count_obj;

    private Image img;

    private new Text name;

    private Text count;

    // Start is called before the first frame update
    void Awake()
    {
        img = img_obj.GetComponent<UnityEngine.UI.Image>();
        name = obj_name_obj.GetComponent<Text>();
        count = obj_count_obj.GetComponent<Text>();
    }

    public void UpdateStats(string obj_id, int obj_count)
    {
        FlowerData data = Resources.Load("Flowers/"+obj_id) as FlowerData;
        if (data == null)
        {
            data = Resources.Load("Flowers/Placeholder") as FlowerData;
            name.text = obj_id;
        }
        else {
            name.text = data.flower_name;
        }
        img.sprite = data.image;
        count.text = obj_count.ToString();
    }

}

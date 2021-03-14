using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDisplayManager : MonoBehaviour
{
    public static PlayerDisplayManager _instance;
    public static PlayerDisplayManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayerDisplayManager>();
             
                if (_instance == null)
                {
                    return null;
                }
            }
     
            return _instance;
        }
    }

    public List<GameObject> temporary_borders;
    public List<GameObject> claimed_borders;

    public GameObject border_object;

    public GameObject player_info;

    public void ClearTemporary()
    {
        foreach (GameObject o in temporary_borders)
        {
            Destroy(o);
        }

        temporary_borders.Clear();
    }

    public void BuildTemporary(List<Hex> to_highlight)
    {
        foreach (Hex h in to_highlight)
        {
            GameObject new_border = Instantiate(border_object, h.transform.position, Quaternion.identity, h.transform);
            temporary_borders.Add(new_border);
        }
    }

    public void AddClaimHighlight(Hex to_highlight)
    {
        GameObject new_border = Instantiate(border_object, to_highlight.transform.position, Quaternion.identity, to_highlight.transform);
        new_border.GetComponent<SpriteRenderer>().color = to_highlight.owner.player_color;
        claimed_borders.Add(new_border);
    }

    public void SetCurrentPlayerInfo(string player_name, Color player_color) 
    {
        TMP_Text name = player_info.GetComponentInChildren<TMP_Text>();
        Image background = player_info.GetComponent<Image>();

        name.text = player_name;
        name.color = player_color;
        background.color = player_color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    public static UIManager _instance;
    public static UIManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIManager>();
             
                if (_instance == null)
                {
                    return null;
                }
            }
     
            return _instance;
        }
    }

    public Vector2 target_position;
    private Vector2 closed_position;
    private Vector2 open_position = new Vector2(-420,-20);
    private float closed_rotation = 90;
    private float open_rotation = 270;
    public float target_rotation;
    
    public bool is_open = false;

    private float animation_timer = 0f;
    public float transition_time;
    
    public GameObject menu_popout;
    private RectTransform rt;
    public GameObject popout_arrow;


    // Start is called before the first frame update
    void Start()
    {
        rt = menu_popout.GetComponent<RectTransform>();
        closed_position = rt.anchoredPosition;
        target_position = closed_position;
        target_rotation = closed_rotation;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        animation_timer = Mathf.Clamp(animation_timer + Time.deltaTime,0f,transition_time);
        rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition,
            target_position, animation_timer / transition_time);
        popout_arrow.transform.localRotation = Quaternion.Euler(new Vector3(0,0,
            Mathf.Lerp(popout_arrow.transform.localEulerAngles.z,target_rotation,animation_timer/transition_time)));
    }

    public void ToggleMenu()
    {
        if (is_open)
        {
            target_position = closed_position;
            target_rotation = closed_rotation;
        }
        else
        {
            target_position = open_position;
            target_rotation = open_rotation;
        }

        animation_timer = 0f;
        is_open = !is_open;
    }
}

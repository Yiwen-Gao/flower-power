using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScript : MonoBehaviour
{
    public static CameraScript _instance;
    public static CameraScript Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CameraScript>();
             
                if (_instance == null)
                {
                    return null;
                }
            }
     
            return _instance;
        }
    }

    private Camera cam;

    private Vector3 target_position;

    private bool should_go_to_target = true;

    public float camera_pan_speed;

    public float min_cam_size;
    public float max_cam_size;
    public float zoom_speed;

    public float size_scale_factor;
    public float default_size;

    private float target_zoom;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Smoothly moving to target
        if (should_go_to_target)
        {
            transform.position = Vector3.Lerp(transform.position, target_position, Time.deltaTime * 2f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, target_zoom, Time.deltaTime * 2f);
        }
        
        // Panning camera
        Vector3 resultant = Vector3.zero;
        if (Input.GetAxis("Horizontal") != 0)
        {
            resultant += Vector3.right*Input.GetAxis("Horizontal");
            should_go_to_target = false;
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            resultant += Vector3.up*Input.GetAxis("Vertical");
            should_go_to_target = false;
        }
        
        transform.Translate(resultant*(Time.deltaTime*camera_pan_speed*(1+cam.orthographicSize*size_scale_factor)));
        
        // Zooming camera

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                cam.orthographicSize =
                    Mathf.Clamp(cam.orthographicSize +
                                Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -zoom_speed, min_cam_size,
                        max_cam_size);
                should_go_to_target = false;
            }
        }
    }

    public void SetTarget(Vector3 pos)
    {
        target_position = pos;
        should_go_to_target = true;
    }

    public void ResetZoom()
    {
        target_zoom = default_size;
        should_go_to_target = true;
    }

    public Camera GetCamera() {
        return cam;
    }
}

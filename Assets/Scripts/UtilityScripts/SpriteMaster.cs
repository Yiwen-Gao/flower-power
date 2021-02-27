using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaster : MonoBehaviour
{
    //this is a singleton-pattern class that controls the sprites used by tiles
    
    public static SpriteMaster _instance;
    public static SpriteMaster Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SpriteMaster>();
             
                if (_instance == null)
                {
                    GameObject container = new GameObject("Sprite Master");
                    _instance = container.AddComponent<SpriteMaster>();
                }
            }
     
            return _instance;
        }
    }
    
    //using the code above, we can get the SpriteMaster from anywhere using SpriteMaster.Instance
    
    public List<SubSpriteList> tile_sprites;
}

[System.Serializable]
public class SubSpriteList
{
    public string type_name; //not used for anything, but makes it easier to keep track of stuff
    public List<Sprite> sprites;
    public List<float> probabilities; //probability of getting each tile type (unused for now)
}

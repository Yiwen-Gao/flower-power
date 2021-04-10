using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEffect : MonoBehaviour {

    public float lifetime;

    private float time_passed = 0f;
    public float maxsize;
    private float startsize;

    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,lifetime);
        sr = this.GetComponent<SpriteRenderer>();
        startsize = transform.localScale.x;
    }

    // Update is called once per frame
    void Update() {
        time_passed += Time.deltaTime;
        sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,Mathf.Lerp(1,0,time_passed/lifetime));
        transform.localScale = Vector3.one * Mathf.Lerp(startsize, startsize*maxsize, time_passed/lifetime);
    }
}

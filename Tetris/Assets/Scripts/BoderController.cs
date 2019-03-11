using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoderController : MonoBehaviour {
    public GameObject left, right, bottom;
    // Use this for initialization
    private void Awake()
    {
        int w = Grid.w;
        int h = Grid.h;
        this.transform.localPosition = new Vector3(w / 2, h / 2, 0);
        left.transform.localPosition = new Vector3(-w / 2 - 0.5f, -0.5f, 0);
        left.transform.localScale = new Vector3(5, h / 2 * 10, 1);
        right.transform.localPosition = new Vector3(w + left.transform.localPosition.x, left.transform.localPosition.y, 0);
        right.transform.localScale = new Vector3(5, h / 2 * 10, 1);
        bottom.transform.localPosition = new Vector3(-0.5f, -h / 2 - 0.5f, 0);
        bottom.transform.localScale = new Vector3(5, w / 2 * 10);
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

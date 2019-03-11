using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawer : MonoBehaviour {

    // Use this for initialization
    public GameObject[] groups;
    public GameObject curGroup;
    public GameObject nextGroup;
    public GameObject posNext;
    private void Awake()
    {
        
    }
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject Spawn()
    {
        if(nextGroup == null)
        {
            int y = Random.Range(0, groups.Length);
            nextGroup = Instantiate(groups[y],
                   posNext.transform.position,
                   Quaternion.identity);

        }
        GameObject curGroup = nextGroup;


        // spawn next group
        int n = groups.Length;
        int i = Random.Range(0, n);
        nextGroup = Instantiate(groups[i],
               posNext.transform.position,
               Quaternion.identity);

        curGroup.transform.position = this.transform.position;

        return curGroup;
    }
}

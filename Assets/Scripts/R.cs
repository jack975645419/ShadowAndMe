using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R : MonoBehaviour {

    public Transform Shd;

    void Awake()
    {
        if(Shd)
        {
            
        }
    }

	// Use this for initialization
	void Start () {
        Transform Inst = Instantiate(Shd, transform.position + new Vector3(0,0,-0.3f),  Quaternion.Euler(-90f, 0, 0));
        Shd InstShd = Inst.GetComponent<Shd>();
        InstShd.R  = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

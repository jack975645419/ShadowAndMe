using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shd : MonoBehaviour {

    public Transform R;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(R) this.transform.position = R.transform.position + new Vector3(0,0,-0.3f);
	}
}

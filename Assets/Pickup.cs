using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    protected BoxCollider m_BoxCollider;
    public Transform ItemClass;
	// Use this for initialization
	void Start () {
        m_BoxCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        PlayerController PC = other.gameObject.GetComponent<PlayerController>();
        if(PC)
        {
            
        }
    }
}

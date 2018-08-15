using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    bool m_Win = false;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trig sth");
        PlayerController PC = other.gameObject.GetComponent<PlayerController>();
        if(PC)
        {
            m_Win = true;
        }
    }

    private void OnGUI()
    {
        if(m_Win)
        {
            GUI.skin.label.fontSize = 50;
            GUI.Label(new Rect(0, 0, 500, 300), "Win!");
            Debug.Log("win");
        }
    }
}

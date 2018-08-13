using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touching : MonoBehaviour {

    Vector2 screenpos = new Vector2();
    float speed = 0.1f;
    protected bool clicked = false;
	// Use this for initialization
	void Start () {
        Input.multiTouchEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        MobileInput();
	}

    void MobileInput()
    {
        if (Input.touchCount <= 0) return;
        if(Input.touchCount==1)
        {
            clicked = true;
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                //记录手指触屏位置
                screenpos = Input.touches[0].position;

            }
            else if(Input.touches[0].phase == TouchPhase.Moved)
            {
                //deltaPosition来表示移动量
                Camera.main.transform.Translate(new Vector3(Input.touches[0].deltaPosition.x * speed, Input.touches[0].deltaPosition.y * speed, 0));
            }
            else if(Input.touches[0].phase==TouchPhase.Ended)
            {
                Debug.Log("h");
            }


        }
    }

    private void OnGUI()
    {
        if(clicked)
        {
            GUI.Label(new Rect(0, 0, 500, 50), "Clicked");
        }
    }
}

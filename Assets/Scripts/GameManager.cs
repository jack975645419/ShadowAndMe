using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTool
{
    /// <summary>
    /// 此函数的v的z的含义是希望对应的游戏世界平面的z值
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3 ScreenToWorld(Vector3 v)
    {
        v.z = Camera.main.WorldToScreenPoint(v).z;
        return Camera.main.ScreenToWorldPoint(v); 
    }
}



public class GameManager : MonoBehaviour {

    public GameObject CommandInputField = null;
    public GameObject[] DebugUIs = null;
    private bool m_ShowDebugUI = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_ShowDebugUI = !m_ShowDebugUI;
            for(int k = 0; k<DebugUIs.Length; k++)
            {
                DebugUIs[k].SetActive(m_ShowDebugUI);
            }
        }
	}
}

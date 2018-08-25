﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : Singleton<Commander> {

    /*// Use this for initialization
	void Start () {
		
	}*/

    // Update is called once per frame
    void Update()
    {

    }

    public void Exec(string s)
    {
        s = s.ToLower();
        Debug.Log("执行命令：" + s);
        string[] cmd = s.Split(' ');
        switch(cmd[0])
        {
            case "print":
                {
                    if (cmd.Length > 1)
                    {
                        Debug.Log(cmd[1]);
                    }
                    break;
                }
            case "printtoscreen":
                {
                    if(cmd.Length>1)
                    {
                        Logger.Instance.LogText.text = "nih";
                    }
                    break;
                }
            case "screeninfo":
                {
                    Debug.Log("screeninfo" + Screen.width +"," + Screen.height);
                    break;
                }
        }
    }
}
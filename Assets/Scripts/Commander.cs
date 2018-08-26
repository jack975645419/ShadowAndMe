using System;
using System.Collections;
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
            case "atan":// y x ，返回的单位是rad
                {
                    if(cmd.Length>=3)
                    {
                        var y = Convert.ToDouble(cmd[1]);
                        var x = Convert.ToDouble(cmd[2]);
                        Debug.Log(string.Format("Atan of (y={0}, x={1}) is {2}", cmd[1], cmd[2], Mathf.Atan2((float)y, (float)x)));
                    }
                    break;
                }
        }
    }
}

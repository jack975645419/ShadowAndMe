﻿using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 句柄管家
/// </summary>
public enum EHandles
{
    //0~10 句柄保留给手指

    EHandle_Temp = -1,
    EHandle_TimeInfo = 11,
    EHandle_ErrorAngle = 12,
}

public class Logger : Singleton<Logger> {

    public Text LogText;
    //句柄-展示的字串 的映射
    private Dictionary<int, string> InfoToShow = new Dictionary<int, string>();
    //句柄-计时器 的映射，与上面的字典对应
    private Dictionary<int, SingleTimer> TimersForInfoShow = new Dictionary<int, SingleTimer>();
    //注意事项：句柄0-10保留给手指信息打印
    //句柄-LineRenderer
    private Dictionary<int, LineRenderer> Lines = new Dictionary<int, LineRenderer>();

    public GameObject Drawer = null;
    
    //维护每一个Line对应的计时器
    Dictionary<LineRenderer, SingleTimer> TimerForLines = new Dictionary<LineRenderer, SingleTimer>();

    public override void Start()
    {
        base.Start();
        EasyTouch.On_Swipe += ShowGesture;
        EasyTouch.On_SwipeStart += ShowGesture;
        EasyTouch.On_SwipeEnd += ShowGesture;
        EventManager.Register(EMessageID.Msg_Hit, OnHit);
        EventManager.Register(EMessageID.Msg_Miss, OnMiss);
    }
    private void Update()
    {
        LogInfoToScreen("Time:" + Time.time, (int)EHandles.EHandle_TimeInfo);
    }


    /// <summary>
    /// 根据句柄在屏幕上显示信息，如果句柄号是-1，则表示是临时信息。同一句柄后显示的信息将会覆盖已有的信息。
    /// </summary>
    /// <param name="s">需要显示的信息</param>
    /// <param name="info_handle">该信息的句柄</param>
    /// <param name="time">该信息的寿命</param>
    public void LogInfoToScreen(string s, int info_handle = (int)EHandles.EHandle_Temp, float time = 5)
    {
        InfoToShow[info_handle] = s;
        if(TimersForInfoShow.ContainsKey(info_handle))
        {
            TimersForInfoShow[info_handle].Interval = time;
            TimerManager.Instance.StartTimer(TimersForInfoShow[info_handle]);
        }
        else
        {
            TimersForInfoShow.Add(
                info_handle, 
                TimerManager.Instance.SetNewTimer(time, RemoveInfoToShow, false, false, info_handle)
                );
        }
        UpdateInfoText();
    }

    public void RemoveInfoToShow(params object[] p)
    {
        InfoToShow.Remove((int)p[0]);
        UpdateInfoText();
    }

    public void UpdateInfoText()
    {
        string info = "Info:\n";
        foreach (var a in InfoToShow)
        {
            info += a.Value + '\n';
        }
        LogText.text = info;
    }


    public void DrawLine(LineRenderer line, Vector3[] screenPositions, Color? color = null, float lifeTime = 5000)
    {
        for(int k = 0; k<screenPositions.Length; k++)
        {
            screenPositions[k] = MTool.ScreenToWorld(screenPositions[k]);
        }

        //绘制
        line.positionCount = screenPositions.Length;
        line.SetPositions(screenPositions);
        line.enabled = true;
        if (color == null) color = Color.red;
        line.startColor = (Color)color;
        line.endColor = (Color)color;
        line.startWidth = 0.14f;

        //设置倒计时关闭
        if(!TimerForLines.ContainsKey(line))
        {
            SingleTimer timer = new SingleTimer(lifeTime, DisableLine, false, false, line);
            TimerForLines.Add(line, timer);
        }
        TimerForLines[line].Interval = lifeTime;
        TimerManager.Instance.StartTimer(TimerForLines[line]);
    }

    public void DrawLine(int line_handle, Vector3[] screenPositions, Color? color = null, float lifeTime = 5000)
    {
        if(!Lines.ContainsKey(line_handle))
        {
            Lines.Add(line_handle, Instantiate(Drawer).GetComponent<LineRenderer>());
        }
        DrawLine(Lines[line_handle], screenPositions, color, lifeTime);
    }


    public void DisableLine(params object[] Params)
    {
        var line = Params[0] as LineRenderer;
        line.enabled = false;
    }


    public void LogHelloworld(Gesture gesture)
    {
        Debug.Log("HelloWorld" + gesture.startPosition);
        
    }


    public void OnTouchDown(Gesture g)
    {
        
    }
    public void ShowGesture(Gesture g)
    {
        Debug.Log(string.Format("当前TouchDown信息：\ng.actionTime:{0}\ng.fingerIndex:{1}\ng.pickedObject:{2}\ng.position:{3}\ng.swipeLength:{4}\ng.swipeVector:{5}\ng.touchCount:{6}\ng.twistAngle:{7}\ng.twoFingerDistance:{8}\n\n", g.actionTime, g.fingerIndex, g.pickedObject != null ? g.pickedObject.ToString() : "null", g.position.ToString(), g.swipeLength, g.swipeVector.ToString(), g.touchCount, g.twistAngle, g.twoFingerDistance));
        
        DrawLine( g.fingerIndex , new Vector3[] { g.startPosition, g.position }, Color.yellow, 2);

        if(!InfoToShow.ContainsKey(g.fingerIndex))
        {
            InfoToShow.Add(g.fingerIndex, "");
        }

        LogInfoToScreen("Finger" + g.fingerIndex + ":" + g.position.ToString(),
            g.fingerIndex,
            0.2f);
    }

    public void Assert(bool l)
    {
        if(!l)
        {
            Debug.LogError("__error__");
        }
    }

    public void OnHit(Msg msg)
    {
        LogInfoToScreen("成功击中，误差" + (float)msg.Params[2], (int)EHandles.EHandle_ErrorAngle);
    }
    public void OnMiss(Msg msg)
    {
        LogInfoToScreen("未击中，误差" + (float)msg.Params[2], (int)EHandles.EHandle_ErrorAngle);

    }
    

}

using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : Singleton<Logger> {

    public Text LogText;
    //句柄-展示的字串 的映射
    private Dictionary<int, string> InfoToShow = new Dictionary<int, string>();
    //句柄-计时器 的映射，与上面的字典对应
    private Dictionary<int, SingleTimer> TimersForInfoShow = new Dictionary<int, SingleTimer>();
    //注意事项：句柄0-10保留给手指信息打印


    /// <summary>
    /// 根据句柄在屏幕上显示信息，如果句柄号是-1，则表示是临时信息。同一句柄后显示的信息将会覆盖已有的信息。
    /// </summary>
    /// <param name="s">需要显示的信息</param>
    /// <param name="info_handle">该信息的句柄</param>
    /// <param name="time">该信息的寿命</param>
    public void LogInfoToScreen(string s, int info_handle = -1, float time = 5)
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

    private void Update()
    {
        //Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1));

        
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

    public LineRenderer DrawLine(Vector3 start, Vector3 end, Color? color = null, float lifeTime = 5000)
    {
        var line = gameObject.AddComponent<LineRenderer>();
        line.SetPositions(new Vector3[] { start, end });
        if (color == null) color = Color.red;
        line.startColor = (Color)color;
        line.endColor = (Color)color;
        Destroy(line, lifeTime);
        return line;
    }



    public void LogHelloworld(Gesture gesture)
    {
        Debug.Log("HelloWorld" + gesture.startPosition);
        
    }

    public override void Start()
    {
        base.Start();
        EasyTouch.On_Swipe += ShowGesture;
        EasyTouch.On_SwipeStart += ShowGesture;
        EasyTouch.On_SwipeEnd += ShowGesture;
        //EasyTouch.On_TouchDown += LogHelloworld;
    }

    public void OnTouchDown(Gesture g)
    {
        
    }
    public void ShowGesture(Gesture g)
    {
        Debug.Log(string.Format("当前TouchDown信息：\ng.actionTime:{0}\ng.fingerIndex:{1}\ng.pickedObject:{2}\ng.position:{3}\ng.swipeLength:{4}\ng.swipeVector:{5}\ng.touchCount:{6}\ng.twistAngle:{7}\ng.twoFingerDistance:{8}\n\n", g.actionTime, g.fingerIndex, g.pickedObject != null ? g.pickedObject.ToString() : "null", g.position.ToString(), g.swipeLength, g.swipeVector.ToString(), g.touchCount, g.twistAngle, g.twoFingerDistance));

        Debug.DrawLine(Camera.main.ScreenToWorldPoint((Vector3)g.startPosition), Camera.main.ScreenToWorldPoint((Vector3)g.position));

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

}

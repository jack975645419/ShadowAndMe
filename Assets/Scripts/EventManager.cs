﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMessageID
{
    Msg_AnimShouldTransform
}

//消息报文
public class Msg
{
    public EMessageID MessageID;
    public object[] Params = null;
    public Msg(EMessageID MessageID, params object[] Params)
    {
        this.MessageID = MessageID;
        this.Params = Params;
    }
}


public class EventManager : Singleton<EventManager> {
    
    public delegate void OnMessageEvent(Msg msg);
    //消息寄存器
    private Dictionary<EMessageID, List<OnMessageEvent>> m_Dict_EventRegister;

    public void Register(EMessageID msg_id, OnMessageEvent e)
    {
        if (!m_Dict_EventRegister.ContainsKey(msg_id))
        {
            m_Dict_EventRegister.Add(msg_id, new List<OnMessageEvent>());
        }
        if(!m_Dict_EventRegister[msg_id].Contains(e))
        {
            m_Dict_EventRegister[msg_id].Add(e);
        }
    }

    public void Unregister(OnMessageEvent e)
    {
        foreach (var i in m_Dict_EventRegister)
        {
            if (i.Value.Contains(e))
            {
                i.Value.Remove(e);
            }
        }
    }

    public void Send(Msg msg)
    {
        if (m_Dict_EventRegister.ContainsKey(msg.MessageID))
        {
            List<OnMessageEvent> events = m_Dict_EventRegister[msg.MessageID];
            for (int k = 0; k < events.Count; k++)
            {
                events[k](msg);
            }
        }
    }
}

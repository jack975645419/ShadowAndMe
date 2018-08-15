

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    //是否处于安卓设备
    protected bool m_IsInAndroid = false;

    //触屏起始位置/鼠标左击起始位置，仅在IsClicked下有效
    protected Vector2 m_BeginningScreenPos = new Vector2();
    
    //触屏相较于起始位置的相对位移/鼠标按下时相较于鼠标起始位置的相对位移，仅在IsClicked下有效
    protected Vector2 m_DeltaFromTouchBeganOn;

    /// <summary>
    /// 左右滑动有效X阈值：
    /// 按下屏幕后左右拖动时，只有当拖动距离的绝对值大于此值时才算触发左/右滑动操作
    /// 鼠标的对应操作是拖拽操作
    /// </summary>
    public float ValidXThreshold = 5;

    /// <summary>
    /// LeftPad/RightPad的有效域：
    /// LeftPad是指左侧操控区域，RightPad是指跳跃操控的区域
    /// </summary>
    protected Rect LeftPadRect = new Rect(0, 0, 0.5f * Screen.width, 1 * Screen.height);
    protected Rect RightPadRect = new Rect(0.5f * Screen.width, 0, 0.5f * Screen.width, 1 * Screen.height);
    
    
    /// <summary>
    /// 
    /// </summary>

    // Use this for initialization
    void Start () {
        PlatformInit();
        
    }

    //平台初始化
    void PlatformInit()
    {
#if UNITY_ANDROID
        m_IsInAndroid = true;
#endif

        string platform;
#if UNITY_EDITOR
        m_IsInAndroid = false;
        platform = "hi,大家好,我是在unity编辑模式下";
#elif UNITY_IPHONE
       platform="hi，大家好,我是IPHONE平台";  
#elif UNITY_ANDROID
       platform="hi，大家好,我是ANDROID平台";  
#elif UNITY_STANDALONE_WIN
       platform="hi，大家好,我是Windows平台";  
#endif
        Debug.Log("Current Platform:" + platform);

        Input.multiTouchEnabled = true;
    }

    // Update is called once per frame
    void Update () {

	}

    protected bool m_IsClicked
    {
        get
        {
            return m_IsInAndroid ? Input.touchCount > 0 : Input.GetMouseButton(0);
        }
    }

    //开始触屏/左击按下
    protected bool m_TouchBegin
    {
        get
        {
            return m_IsInAndroid ?
                Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began
                : Input.GetMouseButtonDown(0);
        }
    }

    //触屏拖拽中/鼠标按住拖拽
    protected bool m_TouchMoving
    {
        get
        {
            return m_IsInAndroid ? Input.touchCount > 0 &&
                Input.touches[0].phase == TouchPhase.Moved
                : Input.GetMouseButton(0);
        }
    }

    //触屏结束/左击起
    protected bool m_TouchEnd
    {
        get
        {
            return m_IsInAndroid ? Input.touchCount > 0 &&
                Input.touches[0].phase == TouchPhase.Ended
                : Input.GetMouseButtonUp(0);
        }
    }

    //触屏位置/鼠标位置（仅按下时有意义）
    protected Vector2 m_TouchPosition
    {
        get
        {
            if(m_IsInAndroid)
            {
                if(Input.touchCount>0)
                {
                    return Input.touches[0].position;
                }
            }
            else
            {
                if(Input.GetMouseButton(0))
                {
                    return (Vector2)Input.mousePosition;
                }
            }
            return Vector2.zero;
        }
    }
        
    public void UpdateInput()
    {
        if (m_TouchBegin)
        {
            m_BeginningScreenPos = m_TouchPosition;
        }
        else if(m_TouchMoving)
        {
            m_DeltaFromTouchBeganOn = m_TouchPosition - m_BeginningScreenPos;
        }
        else if(m_TouchEnd)
        {
            m_BeginningScreenPos = Vector2.zero;
            m_DeltaFromTouchBeganOn = Vector2.zero;
        }
    }

    public bool IsLeftInput {
        get
        {
            return m_IsClicked && m_DeltaFromTouchBeganOn.x < -ValidXThreshold && LeftPadRect.Contains(m_BeginningScreenPos) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        }
    }
    public bool IsRightInput {
        get
        {
            return m_IsClicked && m_DeltaFromTouchBeganOn.x  > ValidXThreshold && LeftPadRect.Contains(m_BeginningScreenPos) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        }
    }

    public bool IsJumping = false;

    public bool IsJumpBeginning {
        get
        {
            if( m_TouchBegin && RightPadRect.Contains( m_BeginningScreenPos ) || Input.GetKeyDown(KeyCode.Space) )
            {
                IsJumping = true;
                return true;
            }
            return false;
        }
    }
    public bool IsJumpEnd
    {
        get
        {
            

            if (IsJumping && m_TouchEnd || Input.GetKeyUp(KeyCode.Space))
            {
                IsJumping = false;
                return true;
            }
            return false;
        }

    }
}

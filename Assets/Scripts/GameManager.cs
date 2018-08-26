using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public static void CalculateABCForParabola(ref float a, ref float b, ref float c, float YIntercept, Vector2 Peak)
    {
        c = YIntercept;
        float x0 = Peak.x;
        float y0 = Peak.y;
        a = (c - y0) / x0 / x0;
        b = -2 * a * x0;
    }
    public static void CalculateBCForStraightLine(ref float a_To_Zero, ref float b, ref float c, float YIntercept, Vector2 pointPassedBy)
    {
        a_To_Zero = 0;

        float x0 = pointPassedBy.x;
        float y0 = pointPassedBy.y;
        c = YIntercept;
        b = (y0 - c) / x0;
    }

    public static Vector3 NormalizedToScreen(Vector3 v)
    {
        v.x *= (float)GameManager.Instance.ScreenWidth;
        v.y *= (float)GameManager.Instance.ScreenHeight;
        return v;
    }
    public static Vector3[] NormalizedToScreen(Vector3[] v)
    {
        for(int k = 0; k<v.Length; k++)
        {
            v[k].x *= (float)GameManager.Instance.ScreenWidth;
            v[k].y *= (float)GameManager.Instance.ScreenHeight;
        }
        return v;
    }

    public static Vector3 NormalizedToWorld(Vector3 v)
    {
        v = NormalizedToScreen(v);
        v = ScreenToWorld(v);
        return v;
    }

    /// <summary>
    /// 根据斜率得到旋转值，未检查
    /// </summary>
    /// <returns></returns>
    public static Quaternion GetQuaternionByGradient(float K)
    {
        return Quaternion.LookRotation(new Vector3(1, K, 0), Vector3.back);
    }

    /// <summary>
    /// 函数特性需要测试，未检查[wenjiezou]tocheck
    /// </summary>
    /// <returns>弧度制角度，取值范围 in the interval [-pi/2,+pi/2] radians </returns>
    public static float GetRotationAngleByGradient(float K)
    {
        return Mathf.Atan(K);
    }

    /// <summary>
    /// 从Vector2转化为角度制角度
    /// </summary>
    /// <returns></returns>
    public static float Vector2Deg(Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 给定角度得到方向向量
    /// </summary>
    public static Vector2 Deg2Vector2(float angleInDeg)
    {
        return new Vector2(Mathf.Cos(angleInDeg * Mathf.Deg2Rad), Mathf.Sin(angleInDeg * Mathf.Deg2Rad));
    }

    /// <summary>
    /// 获得两个角度之间的误差，如+179°和-179°的误差是2°
    /// </summary>
    public static float GetErrorBetween(float angle1InDeg, float angle2InDeg)
    {
        Logger.Instance.Assert(angle1InDeg >= -180.0f && angle1InDeg <= 180.0f && angle2InDeg >= -180.0f && angle2InDeg <= 180.0f);
        return Mathf.Min(Mathf.Abs(angle1InDeg - angle2InDeg + 360.0f),
            Mathf.Abs(angle1InDeg - angle2InDeg),
            Mathf.Abs(angle1InDeg - angle2InDeg - 360.0f)
            );
    }
}


public class GameManager : Singleton<GameManager> {

    public GameObject CommandInputField = null;
    public GameObject[] DebugUIs = null;
    private bool m_ShowDebugUI = true;

    public Table_Touzhiwu m_TableTouzhiwu = null;
    public Table_Paowuxian m_TablePaowuxian = null;
    public Table_Hits m_TableHits = null;
    public float? ScreenWidth = 0;
    public float? ScreenHeight = 0;
    [Tooltip("调试时绘制抛物线的精细程度，建议取值0.08，值越小越精确"), Range(0.001f, 0.2f)]
    public float PrecisionOfDebugPaintTrack = 0.08f;

    public float MusicStartTime = 0.0f;
    [Tooltip("泼墨效果")]
    public GameObject InkEffect = null;
    public Collider2D ForbiddenZoneC2D = null;

    public override void Start()
    {
        base.Start();
#if UNITY_EDITOR
        var gameCamera = Camera.main.GetComponent<GameCamera>();
        var editorScreenSize = gameCamera.GetScreenPixelDimensions();
        ScreenWidth = editorScreenSize.x;
        ScreenHeight = editorScreenSize.y;

#else
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
#endif

        EventManager.Register(EMessageID.Msg_Hit, OnHit);

        //EasyTouch.On_Swipe += FingerRaycastDetect;
        Debug.Log("screeninfo:" + Screen.width + "," + Screen.height);
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.Tab))
        {
            m_ShowDebugUI = !m_ShowDebugUI;
            for(int k = 0; k<DebugUIs.Length; k++)
            {
                DebugUIs[k].SetActive(m_ShowDebugUI);
            }
        }
	}

    public GameObject m_Drawer = null;

    public void FingerRaycastDetect(Gesture g)
    {
        Ray ray = Camera.main.ScreenPointToRay(g.position);
        if(m_Drawer==null)
            m_Drawer = Instantiate(Logger.Instance.Drawer);
        var line = m_Drawer.GetComponent<LineRenderer>();
        line.SetPositions(new Vector3[] { ray.origin, ray.direction * 20 + ray.origin });
        line.enabled = true;

        



        /*
        RaycastHit rayHit;
        int layerOfTouzhiwu = 1 << LayerMask.NameToLayer("Touzhiwu");
        if(Physics.Raycast(ray, out rayHit, layerOfTouzhiwu))
        {
            Debug.Log("i" + rayHit.collider.gameObject.name);
        }*/
    }

    public void OnHit(Msg msg)
    {
        Vector3 v = (Vector3)msg.Params[0];
        var inkEffect = Instantiate(InkEffect, v, Quaternion.identity);
        Vector3 direction = (Vector3)(Vector2)msg.Params[1];
        inkEffect.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        //inkEffect.

        Destroy(inkEffect, 1.5f);
    }


    public void RefreshTables()
    {
        m_TableTouzhiwu.RefreshOnEditor();
        m_TablePaowuxian.RefreshOnEditor();
        m_TableHits.RefreshOnEditor();
    }
    [MenuItem("腾讯迷你游戏项目/RefreshTables")]
    public static void Menu_RefreshTables()
    {
        GameManager.Instance.RefreshTables();
    }

    

}

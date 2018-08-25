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
    /// <param name="K"></param>
    /// <returns></returns>
    public static float GetRotationAngleByGradient(float K)
    {
        return Mathf.Atan(K);
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

        Debug.Log("screeninfo:" + Screen.width + "," + Screen.height);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableRow_Hits : TableRowBase
{
    [Tooltip("弓箭类型，对应表格Touzhiwu"), Range(0, 10)]
    public int Ammu_ID = 0;
    [Tooltip("抛物线类型，对应表格Paowuxian"), Range(0,10)]
    public int Track_ID = 0;
    [Tooltip("打击点理想打击时间，从BGM播放开始算起"), Range(-0.5f, 700.0f)]
    public float ExpectedTime = 1.0f;
    [Tooltip("打击点理想X坐标，单位是NormalizedScale"), Range(0.0f, 1.0f)]
    public float ExpectedXCoordinate = 0.3f;
    //[Tooltip("打击点理想打击角度，正右方是0度，正上方是90度，范围[-180,180)"),Range(-180.0f, 180.0f)]
    public float ExpectedHitAngle
    {
        get
        {
            if(m_Track!=null)
            {
                var ans = MTool.GetRotationAngleByGradient( m_Track.GetGradientByX(ExpectedXCoordinate)) * Mathf.Rad2Deg  + 90.0f;
                return MTool.NormalizeAngleBetween_n180top180(ans);
            }
            return 0;
        }
    }

    public float GetCurrentExpectedHitAngle()
    {
        if (m_Track != null)
        {
            var ans = MTool.GetRotationAngleByGradient(m_Track.GetGradientByX(GetCurXByCurTime())) * Mathf.Rad2Deg + 90.0f;
            return MTool.NormalizeAngleBetween_n180top180(ans);
        }
        return 0;
    }

    
    [Tooltip("告警时间，在完美打击时间前若干秒时，开始告警"), Range(0.2f, 3.0f)]
    public float WarningTime = 1.0f;
    [Tooltip("横方向速度，单位是NormalizedScale/s，为0.1时需要从屏幕左侧到右侧需要耗时10s"), Range(0.001f, 5.0f)]
    public float XVelocity = 0.1f;


    protected TableRow_Paowuxian m_Track = null;
    public TableRow_Paowuxian Track
    {
        get
        {
            Refresh(); 
            return m_Track;
        }
    }
    public void Refresh()
    {
        Ammu_ID = Mathf.Min(Ammu_ID, GameManager.Instance.m_TableTouzhiwu.GetCount() - 1);
        Track_ID = Mathf.Min(Track_ID, GameManager.Instance.m_TablePaowuxian.GetCount() - 1);

        m_Track = GameManager.Instance.m_TablePaowuxian.GetValue(Track_ID);
    }

    public float GetCurXByCurTime()
    {
        //利用当前时间（从BGM开始起计算的时间）获得当前的x，y值
        var curTime = Time.time - GameManager.Instance.MusicStartTime;

        //和完美击打点的时间差
        var deltaTimeWithExpectedTime = curTime - ExpectedTime;

        //和完美击打点的x距离差
        var deltaX = deltaTimeWithExpectedTime * XVelocity;

        //此时的位置x值
        var curX = ExpectedXCoordinate + deltaX;

        return curX;
    }

    /// <summary>
    /// 归一化坐标下的理想打击点
    /// </summary>
    public Vector3 GetExpectedPosInNormalized()
    {
        return Track.GetPointByX(ExpectedXCoordinate);
    }
    public Vector3 GetExpectedPosInWorld()
    {
        return MTool.NormalizedToWorld(GetExpectedPosInNormalized());
    }

}
public class Table_Hits : TableBase<TableRow_Hits> {

    [Tooltip("误差角度，角度制单位，在理想打击角度的基础上±AllowedErrorAngle内均算有效打击"), Range(-180, 180)]
    public float AllowedErrorAngle = 45.0f;

    public override void RefreshOnEditor()
    {
        base.RefreshOnEditor();
        for(int k = 0; k<GetCount(); k++)
        {
            Dict[k].Refresh();
        }
        
        ExpectedAngleDrawerManager.Instance.OnRefreshToShowAngles();
    }

    public void GenerateRandomly(int n)
    {
        Dict.Clear();
        for(int k = 0; k<n; k++)
        {
            var r = new TableRow_Hits();
            r.Id = k;
            r.Ammu_ID = Random.Range(0, GameManager.Instance.m_TableTouzhiwu.GetCount());
            r.Track_ID = Random.Range(0, GameManager.Instance.m_TablePaowuxian.GetCount());
            r.ExpectedTime = k * 2.0f + Random.Range(-2.0f, +2.0f);
            r.ExpectedTime = Mathf.Max(3.0f, r.ExpectedTime);
            r.ExpectedXCoordinate = Random.Range(0.1f, 0.9f);
            r.XVelocity = Random.Range(0.02f, 0.5f);
            Dict.Add(r);
        }
    }

}

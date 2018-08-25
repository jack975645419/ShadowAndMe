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
    [Tooltip("打击点完美打击时间，从BGM播放开始算起"), Range(-0.5f, 700.0f)]
    public float ExpectedTime = 1.0f;
    [Tooltip("打击点的X坐标，单位是NormalizedScale"), Range(0.0f, 1.0f)]
    public float ExpectedXCoordinate = 0.3f;
    [Tooltip("告警时间，在完美打击时间前若干秒时，开始告警"), Range(0.2f, 3.0f)]
    public float WarningTime = 1.0f;
    [Tooltip("横方向速度，单位是NormalizedScale/s，为0.1时需要从屏幕左侧到右侧需要耗时10s"), Range(0.001f, 5.0f)]
    public float XVelocity = 0.1f;

    public float GetCurIdealXByCurTime()
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
    /*[Tooltip("打击点完美打击方向")]
    public Vector2 ExpectedHitVector;*/
}
public class Table_Hits : TableBase<TableRow_Hits> {

    
}

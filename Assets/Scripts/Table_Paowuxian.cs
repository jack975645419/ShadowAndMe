using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableRow_Paowuxian :TableRowBase
{
    [Tooltip("是否是抛物线类型")]
    public bool IsParabola = true;

    [Tooltip("抛物线的Y截距")]
    public float YIntercept = 0.8f;

    [Tooltip("抛物线的最高点或直线的经过点")]
    public Vector2 Peak = new Vector2(0.1f, 0.9f);

    private float a, b, c;
    public void RefreshABC()
    {
        //抛物线情况
        if (IsParabola)
        {
            MTool.CalculateABCForParabola(ref a, ref b, ref c, YIntercept, Peak);
        }
        //直线情况
        else
        {
            //如果是直线，a是0
            MTool.CalculateBCForStraightLine(ref a, ref b, ref c, YIntercept, Peak);
        }
    }

    public float GetYByX(float X)
    {
        //如果是直线，a是0
        return a * X * X + b * X + c;
    }

    public Vector3 GetPointByX(float x)
    {
        return new Vector3(x, GetYByX(x), 0);
    }

    public Vector3[] GetPointsByX(float xFrom, float xTo, float xInterval)
    {
        List<Vector3> ans = new List<Vector3>();
        for (float x = xFrom; x <= xTo; x += xInterval)
        {
            ans.Add(GetPointByX(x));
        }
        return ans.ToArray();
    }

}

public class Table_Paowuxian : TableBase<TableRow_Paowuxian> {
    /*

        // Use this for initialization
        public override void Start () {

        }

        // Update is called once per frame
        void Update () {

        }*/

    public override void RefreshOnEditor()
    {
        base.RefreshOnEditor();

        GameManager.Instance.Start();
        for (int k = 0; k<GetCounts(); k++)
        {
            GetValue(k).RefreshABC();
        }

        TrackPainterManager.Instance.ReconstructAllLines();
    }




}

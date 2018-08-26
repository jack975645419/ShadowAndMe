using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 投掷物产生器
/// </summary>
public class AmmuGenerator : Singleton<AmmuGenerator> {

    //当一个Hit的理论X位置大于等于此距离时，将会产生对应投掷物于它所处的轨迹的该位置处，单位均是归一化单位
    public float XToBeGeneratedFrom = -0.1f;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    protected List<int> HitIDGenerated = new List<int>();

    // Update is called once per frame
    void Update () {
        //在打击点来临前5秒产生投掷物
        var tb = GameManager.Instance.m_TableHits;
        for(int k = 0; k<tb.GetCount(); k++)
        {
            var hitInfo = tb.GetValue(k);
            if (HitIDGenerated.Contains(hitInfo.Id))
            {
                continue;
            }
            var curX = hitInfo.GetCurXByCurTime();
            if ( curX>=XToBeGeneratedFrom )
            {
                var ammuInfo = GameManager.Instance.m_TableTouzhiwu.GetValue(hitInfo.Ammu_ID);
                var ammuClass = ammuInfo.Prefab_Ammu;
                var trackInfo = GameManager.Instance.m_TablePaowuxian.GetValue(hitInfo.Track_ID);

                var newAmmu = Instantiate(ammuClass, MTool.ScreenToWorld(new Vector2(curX, trackInfo.GetYByX(curX))), Quaternion.identity).GetComponent<Ammu>();
                newAmmu.HitInfo = hitInfo;

                HitIDGenerated.Add(hitInfo.Id);
            }
        }
	}
}

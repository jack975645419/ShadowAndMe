using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ammu : MonoBehaviour {

    protected Rigidbody2D m_R2D = null;
    [System.NonSerializedAttribute]
    public TableRow_Hits HitInfo = null;

	// Use this for initialization
	void Start () {
        m_R2D = GetComponent<Rigidbody2D>();
        m_R2D.bodyType = RigidbodyType2D.Kinematic;
	}
	
	// Update is called once per frame
	void Update () {

        if (HitInfo == null) return;

        var curX = HitInfo.GetCurIdealXByCurTime();

        //显示合法
        if (curX >= -0.1f && curX<=1.1f)
        {
            //计算出y值
            var track = GameManager.Instance.m_TablePaowuxian.GetValue(HitInfo.Track_ID);
            var curY = track.GetYByX(curX);
            var curK = track.GetGradientByX(curX);
            //var curQuaternion = MTool.GetQuaternionByGradient(curK);
            var curRotationAngle = MTool.GetRotationAngleByGradient(curK);
            var curPosInWorld = MTool.NormalizedToWorld( new Vector2(curX, curY));
            m_R2D.MovePosition(curPosInWorld);
            m_R2D.rotation = curRotationAngle * Mathf.Rad2Deg;
        }
        //不用显示
        else if(curX>1.1f)
        {
            Destroy(gameObject);
        }
	}
}

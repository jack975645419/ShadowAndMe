using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ammu : MonoBehaviour {

    public Collider2D m_C2D = null;
    protected Rigidbody2D m_R2D = null;
    [System.NonSerializedAttribute]
    public TableRow_Hits HitInfo = null;
    private Vector3 CurrentPositionInWorld;
    public float m_AllowedErrorAngle = 0;
    public bool HasHitPlayer = false;
	// Use this for initialization
	void Start () {
        m_R2D = GetComponent<Rigidbody2D>();
        m_R2D.bodyType = RigidbodyType2D.Kinematic;
        m_C2D = GetComponent<Collider2D>();
        EasyTouch.On_Swipe += OnSwipe;
        m_AllowedErrorAngle = GameManager.Instance.m_TableHits.AllowedErrorAngle;
    }

    // Update is called once per frame
    void Update () {

        if (HitInfo == null) return;

        if(!HasHitPlayer)
        {

            var curX = HitInfo.GetCurXByCurTime();

            //显示合法
            if (curX >= -0.1f && curX <= 1.1f)
            {
                //计算出y值
                var curY = HitInfo.Track.GetYByX(curX);
                var curK = HitInfo.Track.GetGradientByX(curX);
                //var curQuaternion = MTool.GetQuaternionByGradient(curK);
                var curRotationAngle = MTool.GetRotationAngleByGradient(curK);
                CurrentPositionInWorld = MTool.NormalizedToWorld(new Vector2(curX, curY));
                m_R2D.MovePosition(CurrentPositionInWorld);
                m_R2D.rotation = curRotationAngle * Mathf.Rad2Deg;
            }
            //不用显示
            else if (curX > 1.1f)
            {
                Destroy(gameObject);
            }
        }
	}

    public void Init(TableRow_Hits hitInfo)
    {
        HitInfo = hitInfo;
    }

    public void OnSwipe(Gesture g)
    {
        if(m_C2D!=null)
        {
            if(m_C2D.OverlapPoint(MTool.ScreenToWorld( g.position)))
            {
                //产生击打尝试
                Debug.Log("hit");
                var hitVectorDirection = g.swipeVector;
                var hitAngle = MTool.Vector2Deg(hitVectorDirection);

                var errorAngle = MTool.GetErrorBetween(hitAngle, HitInfo.ExpectedHitAngle);
                if(errorAngle<=m_AllowedErrorAngle)
                {
                    Destroy(gameObject);
                    EventManager.Send(new Msg(EMessageID.Msg_Hit, CurrentPositionInWorld, g.swipeVector, errorAngle));
                }
                else
                {
                    EventManager.Send(new Msg(EMessageID.Msg_Miss, CurrentPositionInWorld, g.swipeVector, errorAngle));
                }
            }
        }
    }

    public void HitPlayer()
    {
        HasHitPlayer = true;
        Destroy(gameObject, 2.0f);
    }

}

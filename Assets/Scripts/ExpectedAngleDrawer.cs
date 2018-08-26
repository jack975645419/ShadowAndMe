using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class ExpectedAngleDrawer : MonoBehaviour {

    public LineRenderer m_ExpectedAngleDrawer = null;
    public LineRenderer m_LeftDrawer = null;
    public LineRenderer m_RightDrawer = null;
    public TextMesh m_IdShow = null;

    /* 仅做调试使用
    [Range(0.0f, 180.0f)]
    public float AllowedErrorAngle = 45.0f;
    [Range(-180.0f, 180.0f)]
    public float ExpectedHitAngle = 90.0f;*/
    public TableRow_Hits HitInfo = null;
    
    // Use this for initialization
    void Start () {
        //RefreshWithInfo(TODO);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Refresh()
    {
        transform.position = HitInfo.GetExpectedPosInWorld();

        var offset = MTool.Deg2Vector2(HitInfo.ExpectedHitAngle);
        offset *= 1.0f;//长度

        m_ExpectedAngleDrawer.SetPosition(0, gameObject.transform.position);
        m_ExpectedAngleDrawer.SetPosition(1, gameObject.transform.position + (Vector3)offset);

        offset = MTool.Deg2Vector2(HitInfo.ExpectedHitAngle - GameManager.Instance.m_TableHits.AllowedErrorAngle);
        offset *= 1.0f;//长度
        m_LeftDrawer.SetPosition(0, gameObject.transform.position);
        m_LeftDrawer.SetPosition(1, gameObject.transform.position + (Vector3)offset);

        offset = MTool.Deg2Vector2(HitInfo.ExpectedHitAngle + GameManager.Instance.m_TableHits.AllowedErrorAngle);
        offset *= 1.0f;//长度
        m_RightDrawer.SetPosition(0, gameObject.transform.position);
        m_RightDrawer.SetPosition(1, gameObject.transform.position + (Vector3)offset);
        
        m_IdShow.text = "P" + HitInfo.Id;
    }
}

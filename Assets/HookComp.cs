using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookComp : MonoBehaviour {

    public Transform Generator;
    public Transform PuppetBody;
    public GameObject RopePointClass;
    protected List<GameObject> m_RopePoints = new List<GameObject>();
    public float GenerateRopePointThreshold = 0.25f;
    public float HookInitialVelocityMultiplication = 2.5f;
    public GameObject catched = null;
    [SerializeField, Range(0,1)]
    [Tooltip("该数值表示后一个生成的RopePoint的初始速率是前一者瞬时速率的若干倍")]
    public float RopeVelocityMultiplication = 0.5f;

    public GameObject LastRopePoint
    {
        get
        {
            return m_RopePoints[m_RopePoints.Count - 1];
        }
    }

    [SerializeField, Range(0, 500)]
    [Tooltip("该数值表示收回绳子时候，绳子点回收到人偶手中的施加力的大小")]
    public float AttractionMultiplication = 5f;
	// Use this for initialization
	void Start () {
        m_RopePoints.Add(this.gameObject);
        CircleCollider2D circleCollision2D = this.gameObject.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(circleCollision2D, Generator.gameObject.GetComponent<Collider2D>());
	}
	
	// Update is called once per frame
	void Update () {

        if (catched==null && Vector2.Distance( LastRopePoint.transform.position , Generator.position ) > GenerateRopePointThreshold)
        {
            GameObject newRopePoint = Instantiate(RopePointClass, Generator.position, Quaternion.identity);
            HingeJoint2D newRopePointHingeJoint2D = newRopePoint.GetComponent<HingeJoint2D>();
            newRopePointHingeJoint2D.connectedBody = LastRopePoint.GetComponent<Rigidbody2D>();
            newRopePoint.AddComponent<Line>();
            Line lineOfNewRopePoint = newRopePoint.GetComponent<Line>();

            Rigidbody2D newRopeRigidbody2D = newRopePoint.GetComponent<Rigidbody2D>();
            newRopeRigidbody2D.velocity = LastRopePoint.GetComponent<Rigidbody2D>().velocity * RopeVelocityMultiplication;
            
            
            lineOfNewRopePoint.gameObject1 = newRopePoint;
            lineOfNewRopePoint.gameObject2 = LastRopePoint;

            m_RopePoints.Add(newRopePoint);
        }
        if(catched!=null)
        {
            Rigidbody2D LastRopePointRigidbody2D = LastRopePoint.GetComponent<Rigidbody2D>();
            LastRopePointRigidbody2D.AddForce( AttractionMultiplication * ((Vector2)PuppetBody.position - LastRopePointRigidbody2D.position).normalized );
            if(Vector2.Distance(LastRopePoint.transform.position, PuppetBody.position)<GenerateRopePointThreshold)
            {
                Destroy(LastRopePoint);
                m_RopePoints.Remove(LastRopePoint);
            }
            
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Finish"))
        {
            for(int k = 0; k<m_RopePoints.Count; k++)
            {
                Destroy(m_RopePoints[k].gameObject);
            }
            m_RopePoints = null;
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.CompareTag("Catchable"))
        {
            HingeJoint2D newHingeJoint = this.gameObject.AddComponent<HingeJoint2D>();
            newHingeJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>(); ;
            catched = collision.gameObject;
        }
    }
}

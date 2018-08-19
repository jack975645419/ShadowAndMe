using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementComp : MonoBehaviour {


    protected Rigidbody2D r2d;
    protected Vector2 lastPosition;
    protected Vector2 curPosition;
    protected Vector2 realVelocity;
    public Vector2 TargetVelocity;
    public float raycastDistance = 0.3f;
    public float minGroundNormalY = 0.5f;
    protected Collider2D collider2d;


    //wenjiezou 这种写法表示该变量外界只读
    public bool IsGrounded { get; protected set; }
    public Vector2 groundNormal { get; protected set; }

    private void Awake()
    {
        r2d = GetComponent<Rigidbody2D>();
        curPosition = r2d.position;
        collider2d = GetComponent<Collider2D>();
    }

    // Use this for initialization
    void Start() {
        //Time.timeScale = 0.1f;
    }

    // Update is called once per frame
    void Update() {
    }

    public void MoveByTargetVelocity()
    {
        lastPosition = curPosition;
        curPosition += TargetVelocity * Time.deltaTime;
        r2d.MovePosition(curPosition);//位移发生于此

        realVelocity = (curPosition - lastPosition) / Time.deltaTime;
    }

    //wenjiezou
    /// <summary>
    /// 【2】该函数企图判断是否处于地面，原理是用射线检测
    /// 
    /// Normal表示法方向，groundNormal是撞击点处的法方向，即地面的法方向。
    /// </summary>
    public void UpdateGrounded()
    {
        RaycastHit2D[] hitResults = new RaycastHit2D[5];
        collider2d.Raycast(Vector2.down, hitResults, raycastDistance);

        for (int k = 0; hitResults != null && k < hitResults.Length; k++)
        {
            if (hitResults[k].collider != null && hitResults[k].collider.CompareTag("Finish"))
            {
                groundNormal = hitResults[k].normal;
                if (groundNormal.y > minGroundNormalY)
                {
                    IsGrounded = true;
                    return;
                }
            }
        }
        IsGrounded = false;
    }


    //wenjiezou
    //取某向量在地面的投影
    //语法上注意 ref，表示按引用传入
    public void ProjectOnGround(ref Vector2 velocity)
    {
        Vector2 surface = new Vector2(groundNormal.y, -groundNormal.x);
        if (IsGrounded && Vector3.Cross((Vector3)velocity, (Vector3)surface).z > 0)
        {
            velocity = Vector2.Dot(velocity, surface) * surface;
        }
    }

    public bool IsFalling
    {
        get
        {
            return realVelocity.y < 0;
        }
    }
}

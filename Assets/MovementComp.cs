using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementComp : MonoBehaviour {


    protected Rigidbody2D r2d;
    protected Vector2 nextMovement;
    protected Vector2 lastPosition;
    protected Vector2 curPosition;
    protected Vector2 velocity;
    public float raycastDistance = 0.3f;
    public float minGroundNormalY = 0.5f;

    //wenjiezou 这种写法表示该变量外界只读
    public bool IsGrounded { get; protected set; }
    public Vector2 groundNormal { get; protected set; }

    private void Awake()
    {
        r2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        //Time.timeScale = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateGrounded();
    }

    //wenjiezou 更新位置并记录
    private void FixedUpdate()
    {
        lastPosition = curPosition;
        curPosition = lastPosition + nextMovement;
        r2d.MovePosition(curPosition);//位移发生于此

        nextMovement = Vector2.zero;
        velocity = (curPosition - lastPosition) / Time.fixedDeltaTime;

        Debug.Log("velocity:"+velocity);
    }

    public void Move(Vector2 movement)
    {
        nextMovement += movement;
    }

    //wenjiezou
    /// <summary>
    /// 【2】该函数企图判断是否处于地面，原理是用射线检测
    /// 
    /// Normal表示法方向，groundNormal是撞击点处的法方向，即地面的法方向。
    /// </summary>
    public void UpdateGrounded()
    {
        RaycastHit2D result = Physics2D.Raycast(curPosition, Vector2.down,  raycastDistance);
        
        Debug.DrawRay(curPosition, Vector2.down* raycastDistance, Color.red);
        if(result)
        {
            groundNormal = result.normal;
            if(groundNormal.y>minGroundNormalY)
            {
                IsGrounded = true;
                return;
            }
        }
        IsGrounded = false;
    }


    //wenjiezou
    //取某向量在地面的投影
    //语法上注意 ref，表示按引用传入
    public void ProjectOnGround(ref Vector2 velocity)
    {
        if(IsGrounded)
        {
            Vector2 surface = new Vector2(groundNormal.y, -groundNormal.x);
            velocity = Vector2.Dot(velocity, surface) * surface;
        }
    }
}

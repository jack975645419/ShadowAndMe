using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementComp))]
public class PlayerController : MonoBehaviour {

    protected MovementComp movementComp;

    protected Vector2 velocity;
    public float speed = 5f;
    public float jumpLaunchSpeed = 10f;
    public float gravity = 250f;
    protected bool jumping = false;

    private void Awake()
    {
        movementComp = GetComponent<MovementComp>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    //wenjiezou 
    /// <summary>
    /// 【1】定时更新函数，更新频率50Hz
    /// 下面代码企图决定速度值，因素包括用户的输入、重力、是否贴地
    /// 
    /// 关于地面判断和移动的更底层逻辑，请看movementComponent
    /// </summary>
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            velocity += Vector2.left * speed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            velocity += Vector2.right * speed;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity += Vector2.up * jumpLaunchSpeed;
            jumping = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jumping = false;
        }

        //重力作用
        velocity += gravity * Time.fixedDeltaTime * Vector2.down;
        if (movementComp.IsGrounded && !jumping)
        {
            movementComp.ProjectOnGround(ref velocity);
        }

        movementComp.Move(velocity * Time.fixedDeltaTime);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementComp))]
[RequireComponent(typeof(InputManager))]
public class PlayerController : MonoBehaviour {

    protected MovementComp movementComp;

    protected Vector2 velocity;
    public float speed = 5f;
    public float jumpLaunchSpeed = 10f;
    public float gravity = 250f;

    protected InputManager m_InputManager;

    private void Awake()
    {
        movementComp = GetComponent<MovementComp>();
        m_InputManager = GetComponent<InputManager>();
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
        m_InputManager.UpdateInput();

        


        velocity.x = 0;

        if ( m_InputManager.IsLeftInput )
        {
            velocity += Vector2.left * speed;
        }
        if ( m_InputManager.IsRightInput)
        {
            velocity += Vector2.right * speed;
        }
        if (m_InputManager.IsJumpBeginning)
        {
            velocity += Vector2.up * jumpLaunchSpeed;
            //Debug.Log("jumpbegins");
        }
        else if (m_InputManager.IsJumpEnd)
        {
            velocity.y /= 2.0f;
            //Debug.Log("Jumpend");
        }

        //重力作用
        velocity += gravity * Time.fixedDeltaTime * Vector2.down;

        if (movementComp.IsGrounded && !m_InputManager.IsJumping )
        {
            movementComp.ProjectOnGround(ref velocity);
        }

        movementComp.Move(velocity * Time.fixedDeltaTime);
    }

    



}

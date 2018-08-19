using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementComp))]
[RequireComponent(typeof(InputManager))]
public class PlayerController : MonoBehaviour {

    protected MovementComp movementComp;

    public float speed = 5f;
    public float jumpLaunchSpeed = 10f;
    public float gravity = 250f;
    protected ThrowLogic throwLogic = null;

    protected InputManager m_InputManager;

    private void Awake()
    {
        movementComp = GetComponent<MovementComp>();
        m_InputManager = GetComponent<InputManager>();
        throwLogic = GetComponent<ThrowLogic>();
    }
    // Use this for initialization
    void Start () {
		
	}

    //为了精确而设置的一个判断值，在地面上跳起后 直到JumpInputUp过程中均为true
    protected bool validJumpInput = false;

	// Update is called once per frame
	void Update () {

        if( GetComponent<Rigidbody2D>().bodyType== RigidbodyType2D.Dynamic ) return;

         m_InputManager.UpdateInput();

        movementComp.UpdateGrounded();
        movementComp.TargetVelocity.x = 0;

        //重力作用
        movementComp.TargetVelocity += gravity * Time.deltaTime * Vector2.down;

        if ( m_InputManager.IsLeftInput )
        {
            Debug.Log("L");
            movementComp.TargetVelocity += Vector2.left * speed;
        }
        if ( m_InputManager.IsRightInput)
        {
            Debug.Log("R");
            movementComp.TargetVelocity += Vector2.right * speed;
        }
        if (m_InputManager.IsJumpInputDown && movementComp.IsGrounded)
        {
            Debug.Log("ValidJump");
            movementComp.TargetVelocity += Vector2.up * jumpLaunchSpeed;
            validJumpInput = true;

        }
        else if (m_InputManager.IsJumpInputUp && !movementComp.IsFalling && validJumpInput)
        {
            Debug.Log("ValidEndJump");
            movementComp.TargetVelocity.y /= 2.0f;
            validJumpInput = false;

        }


        if(throwLogic!=null&& throwLogic.hookGameObject!=null&&throwLogic.hookGameObject.GetComponent<HookComp>().catched!=null)
        {
            var hook = throwLogic.hookGameObject.GetComponent<HookComp>();
            movementComp.TargetVelocity = (Vector2)(hook.LastRopePoint.transform.position - transform.position).normalized * speed;
        }

        
        if (movementComp.IsGrounded && (!m_InputManager.IsJumping || movementComp.TargetVelocity.y<0f))
        {
            movementComp.ProjectOnGround(ref movementComp.TargetVelocity);
            validJumpInput = false;
        }

        movementComp.MoveByTargetVelocity();
    }

    //wenjiezou 
    /// <summary>
    /// 【1】定时更新函数，更新频率50Hz
    /// 下面代码企图决定速度值，因素包括用户的输入、重力、是否贴地
    /// 
    /// 关于地面判断和移动的更底层逻辑，请看movementComponent
    /// </summary>

}

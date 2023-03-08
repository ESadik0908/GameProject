using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovement : MonoBehaviour
{
    //Player movement stats  that can be changed in the editor
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float timeToApex = 0.4f;
    [SerializeField] private float moveSpeed = 6;


    private float accelerationTimeAir = 0.2f;
    private float accelerationTimeGround = 0.1f;

    private float moveSmoothing;
    private float gravity;
    private float jumpForce;

    private bool jump = false;

    private float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    bool isDashing = false;

    public float facing { get; private set; }

    Vector2 input;

    public Vector3 velocity;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        ResetGravity();

        facing = 1;
    }

    private void Update()
    {
        if (isDashing) return;
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(input.x != 0 && Mathf.Sign(input.x) != facing)
        {
            facing = Mathf.Sign(input.x);
        }

        //A jump implimented with cyote time and jump buffering, this makes the controls feel more forgiving
        #region Jump
        if (controller.collisions.below)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            coyoteTimeCounter = 0.001f;
            jump = true;
        }

        if (Input.GetButtonUp("Jump") && velocity.y > 0)
        {
            velocity.y = velocity.y * 0.5f;
        }
        #endregion
    }

    void FixedUpdate()
    {
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        
        if (jump)
        {
            velocity.y = jumpForce;
            jump = false;
        }
        
        //Player movement in the x axis with smoothing so the player doesn't come to a sudden stop when changing direction
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref moveSmoothing, (controller.collisions.below)?accelerationTimeGround:accelerationTimeAir);

        if(velocity.y > -50)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        jump = true;
    }

    public void ResetGravity()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        jumpForce = Mathf.Abs(gravity) * timeToApex;
    }

    public void Hover()
    {
        gravity = -1f;
        velocity.y = 0;
    }

    public IEnumerator Dash(float[] dashStats)
    {
        SendMessage("ToggleDashing");
        isDashing = true;
        float dashSpeed = dashStats[0];
        float dashDuration = dashStats[1];
        float gravityResetDelay = dashStats[2];


        gravity = 0f;
        velocity.y = 0f;

        Vector2 oldVel = velocity;
        while (dashDuration > 0)
        {
            velocity.x = dashSpeed * facing;
            dashDuration -= Time.deltaTime;

            // Check for collisions with walls
            if (controller.collisions.left || controller.collisions.right)
            {
                break;
            }

            yield return null;
        }
        isDashing = false;
        yield return new WaitForSeconds(gravityResetDelay);

        ResetGravity();
        SendMessage("ToggleDashing");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovement : MonoBehaviour
{
    //Player movement stats  that can be changed in the editor
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float timeToApex = 0.4f;
    [SerializeField] private float defaultMoveSpeed = 6;
    [SerializeField] private float moveSpeed;

    private float accelerationTimeAir = 0.2f;
    private float accelerationTimeGround = 0.1f;

    private float moveSmoothing;
    private float gravity;
    private float jumpForce;

    public bool jump = false;

    private float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    private bool isDashing = false;

    public float facing { get; private set; }

    private float input;

    private Vector3 velocity;

    private Controller2D controller;

    private PlayerUpgrades playerUpgrades;

    private void Start()
    {
        playerUpgrades = GetComponent<PlayerUpgrades>();
        moveSpeed = defaultMoveSpeed;
        controller = GetComponent<Controller2D>();

        FindAnyObjectByType<Player>().loadEvent += loadPlayer;

        ResetGravity();

        facing = 1;
    }

    private void Update()
    {
        if (isDashing) return;

        moveSpeed = defaultMoveSpeed + playerUpgrades.speed;

        input = Input.GetAxisRaw("Horizontal");

        if (input != 0 && Mathf.Sign(input) != facing)
        {
            facing = Mathf.Sign(input);
        }

        //A jump implimented with cyote time and jump buffering, this makes the controls feel more forgiving

        #region Jumping
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

    private void FixedUpdate()
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

        float targetVelocityX = input * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref moveSmoothing, (controller.collisions.below)?accelerationTimeGround:accelerationTimeAir);

        if(velocity.y > -50)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void ResetGravity()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        jumpForce = Mathf.Abs(gravity) * timeToApex;
    }

    public void Hover()
    {
        gravity = -1f;
        velocity.y = jumpForce / 2;
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

    public Vector3 getVelocity()
    {
        return velocity;
    }

    private void loadPlayer(Player player)
    {
        ResetGravity();
    }

    public float getGravity()
    {
        return -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
    }
}
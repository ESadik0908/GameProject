using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class DoubleJump : MonoBehaviour 
{
    private float cyoteTime = 0.1f;
    private float cyoteTimeCounter;

    private float jumpBufferTime = 0.3f;
    private float jumpBufferCounter;

    private bool jump = false;

    [SerializeField] private int doubleJump;


    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (cyoteTimeCounter > 0f)
        {
            doubleJump = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region Jumping
        if (Input.GetButtonDown("Jump") && (isGrounded() || doubleJump > 0))
        {
            jump = true;
        }

        if (isGrounded())
        {
            cyoteTimeCounter = cyoteTime;
        }
        else
        {
            cyoteTimeCounter -= Time.deltaTime;
        }

        if (jump && doubleJump > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            doubleJump -= 1;
            jump = false;
        }

        if (jump)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        //If you oare on the ground and the jump buffer is ready then jump
        if (jumpBufferCounter > 0f && cyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            jumpBufferCounter = 0;
            jump = false;
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            cyoteTimeCounter = 0f;
        }
        #endregion
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}

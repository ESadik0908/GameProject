using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;

    private float cyoteTime = 0.2f;
    private float cyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [SerializeField] private int doubleJump;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);


        if (isGrounded())
        {
            doubleJump = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isGrounded())
        {
            cyoteTimeCounter = cyoteTime;
            doubleJump = 0;
        }
        else
        {
            cyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && doubleJump < 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            doubleJump += 1;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        if (jumpBufferCounter > 0f && cyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            doubleJump += 1;

            jumpBufferCounter = 0;
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            cyoteTimeCounter = 0f;
        }

        Flip();
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}

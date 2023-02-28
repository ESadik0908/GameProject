using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float timeToApex = 0.4f;
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float accelerationTimeAir = 0.2f;
    [SerializeField] private float accelerationTimeGround = 0.1f;

    private float moveSmoothing;
    private float gravity;
    private float jumpForce;

    private bool jump = false;

    private float cyoteTime = 0.2f;
    private float cyoteTimeCounter;

    private float jumpBufferTime = 0.3f;
    private float jumpBufferCounter;

    Vector2 input;

    Vector3 velocity;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        jumpForce = Mathf.Abs(gravity) * timeToApex;
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (controller.collisions.below)
        {
            cyoteTimeCounter = cyoteTime;
        }
        else
        {
            cyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && (controller.collisions.below || cyoteTimeCounter > 0))
        {
            cyoteTimeCounter = 0;
            jump = true;
        }      
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

        if (Input.GetButtonUp("Jump") && velocity.y > 0)
        {
            velocity.y = velocity.y * 0.5f;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref moveSmoothing, (controller.collisions.below)?accelerationTimeGround:accelerationTimeAir);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
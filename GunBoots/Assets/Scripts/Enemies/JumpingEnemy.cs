using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class JumpingEnemy : MonoBehaviour
{
    private GameObject player;

    public float facing;
    private Controller2D controller;
    private PlayerMovement playerMovement;

    private BoxCollider2D collider;

    private Vector2 bottomLeft;
    private Vector2 bottomRight;

    private Vector3 velocity;

    private float gravity;

    private Vector2[] origins;

    [SerializeField] private float jumpTimer;
    [SerializeField] private float jumpTimerReset;

    [SerializeField] private float speed;
    [SerializeField] private float timeToApex = 0.4f;
    private float jumpForce;

    private bool isJumping = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();
        controller = GetComponent<Controller2D>();
        gravity = playerMovement.getGravity();
        jumpForce = Mathf.Abs(gravity) * timeToApex;
        jumpTimer = jumpTimerReset;
    }

    private void Update()
    {
        if (TimeBody.isRewinding) return;
        float playerSide = player.transform.position.x - transform.position.x;
        float yDifference = Mathf.Abs(player.transform.position.y - transform.position.y);
        float xDifference = Mathf.Abs(player.transform.position.x - transform.position.x);


        facing = Mathf.Sign(playerSide);

        UpdateRaycastOrigins();

        Vector2 rayOrigin = (facing == -1) ? bottomLeft : bottomRight;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

        if (velocity.y > -50)
        {
            velocity.y += gravity * Time.deltaTime;
        }


        if (isJumping && controller.collisions.below)
        {
            velocity = Vector3.zero;
            isJumping = false;
            jumpTimer = jumpTimerReset;
        }


        if (!isJumping && controller.collisions.below && jumpTimer <= 0)
        {
            isJumping = true;
            velocity.x = Mathf.Clamp(xDifference, 0, 5) * facing;
            velocity.y = jumpForce;
        }
        
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (TimeBody.isRewinding) return;
        if ((controller.collisions.above || controller.collisions.below) && !isJumping)
        {
            velocity.y = 0;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bottomLeft = new Vector2(bounds.min.x + 0.1f, bounds.min.y - 0.01f);
        bottomRight = new Vector2(bounds.max.x - 0.1f, bounds.min.y - 0.01f);
    }
}

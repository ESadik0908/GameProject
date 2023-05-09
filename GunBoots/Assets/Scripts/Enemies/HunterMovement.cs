using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HunterMovement : MonoBehaviour
{
    private GameObject player;

    public float facing;
    public float playerAboveOrBelow;
    private Controller2D controller;
    private PlayerMovement playerMovement;

    private BoxCollider2D collider;

    private Vector2 bottomLeft;
    private Vector2 bottomRight;

    private Vector3 velocity;

    private float gravity;

    private Vector2[] origins;

    [SerializeField] private float speed;
    [SerializeField] private float timeToApex = 0.4f;
    private float jumpForce;

    private float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();
        controller = GetComponent<Controller2D>();
        gravity = playerMovement.getGravity();
        jumpForce = Mathf.Abs(gravity) * timeToApex;
    }

    private void Update()
    {
        float playerSide = player.transform.position.x - transform.position.x;
        float playerYLoc = player.transform.position.y - transform.position.y;
        float yDifference = Mathf.Abs(player.transform.position.y - transform.position.y);

        facing = Mathf.Sign(playerSide);
        playerAboveOrBelow = Mathf.Sign(playerYLoc);

        UpdateRaycastOrigins();

        Vector2 rayOrigin = (facing == -1) ? bottomLeft : bottomRight;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

        if (velocity.y > -50)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (yDifference > 5)
        {
            velocity.x = 0;
            return;
        }

        if (controller.collisions.below)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (hit.distance > 1)
        {
            if(playerAboveOrBelow == 1)
            {
                if (coyoteTimeCounter > 0)
                {
                    velocity.y = jumpForce;
                }
                else
                {
                    velocity.x = 0;
                }
            }          
            
        }
        velocity.x = facing * speed;
    }

    private void FixedUpdate()
    {
        if (controller.collisions.above || controller.collisions.below)
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

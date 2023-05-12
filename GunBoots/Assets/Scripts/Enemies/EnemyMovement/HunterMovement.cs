using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This class handles all movement of the hunter enemy type
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
    [SerializeField] private float timeToApex = 0.8f;
    private float jumpForce;

    private float coyoteTime = 0.1f;
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
        if (TimeBody.isRewinding) return;

        float playerSide = player.transform.position.x - transform.position.x;
        float playerYLoc = player.transform.position.y - transform.position.y;
        float yDifference = Mathf.Abs(player.transform.position.y - transform.position.y);
        float xDifference = Mathf.Abs(player.transform.position.x - transform.position.x);
        
        facing = Mathf.Sign(playerSide);

        //Flip the sprite to face the direction of movement
        Vector3 theScale = transform.localScale;
        theScale.x = Mathf.Sign(velocity.x) * 0.7f;
        transform.localScale = theScale;
        
        playerAboveOrBelow = Mathf.Sign(playerYLoc);

        UpdateRaycastOrigins();

        Vector2 rayOrigin = (facing == -1) ? bottomLeft : bottomRight;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

        if (velocity.y > -50)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (xDifference < 0.1)
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

        //Jump if the player is above and close
        if (xDifference < 5)
        {
            if (playerAboveOrBelow == 1 && yDifference > 1)
            {
                if (coyoteTimeCounter > 0)
                {
                    velocity.y = jumpForce;
                }
            }
        }
        
        //jump if we reach the edge of a platform and the player is above
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
        if (TimeBody.isRewinding) return;
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

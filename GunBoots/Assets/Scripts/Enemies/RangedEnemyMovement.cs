using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RangedEnemyMovement : MonoBehaviour
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

    [SerializeField] private float speed;

    private bool moving = false;

    private float moveCooldown;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();
        controller = GetComponent<Controller2D>();
        gravity = playerMovement.getGravity();

        moveCooldown = Random.Range(1f, 5f);
    }

    private void Update()
    {
        if (TimeBody.isRewinding) return;
        float playerSide = player.transform.position.x - transform.position.x;
        float yDifference = Mathf.Abs(player.transform.position.y - transform.position.y);


        facing = Mathf.Sign(playerSide);

        UpdateRaycastOrigins();

        Vector2 rayOrigin = (facing == -1) ? bottomLeft : bottomRight;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

        if (velocity.y > -50)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (yDifference > 5 )
        {
            velocity.x = 0;  
            return;
        }

        if (moveCooldown <= 0)
        {
            moving = true;
            StartCoroutine("MoveRandomly");
            moveCooldown = Random.Range(1, 5);
        }

        if (!moving)
        {
            moveCooldown -= Time.deltaTime;
        }
 
        if (hit.distance > 1)
        {
            velocity.x = 0;
        }
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

    private IEnumerator MoveRandomly()
    {
        if (TimeBody.isRewinding) yield return new WaitForEndOfFrame();
        float xDifference = Mathf.Abs(player.transform.position.x - transform.position.x);

        while (true)
        {
            if (xDifference > 10)
            {
                velocity.x = facing * speed;
                break;
            }

            if (xDifference < 10)
            {
                velocity.x = -facing * speed;
                break;
            }
        }
        
        float delay = Random.Range(1, 5);
        yield return new WaitForSeconds(delay);
        moving = false;
        velocity.x = 0;
    }
}

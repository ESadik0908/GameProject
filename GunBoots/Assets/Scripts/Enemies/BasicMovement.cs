using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BasicMovement : MonoBehaviour
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

    private void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();
        controller = GetComponent<Controller2D>();
        gravity = playerMovement.getGravity();
        
    }

    private void Update()
    {
        float distToPlayer = player.transform.position.x - transform.position.x;

        facing = Mathf.Sign(distToPlayer);

        UpdateRaycastOrigins();

        Vector2 rayOrigin = (facing == -1) ? bottomLeft : bottomRight;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);
        
        velocity.x = facing * speed;
        if (hit.distance > 1)
        {
            velocity.x = 0;
        }
        
        if (velocity.y > -50)
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bottomLeft = new Vector2(bounds.min.x + 0.1f, bounds.min.y - 0.01f);
        bottomRight = new Vector2(bounds.max.x - 0.1f, bounds.min.y - 0.01f);
    }
}

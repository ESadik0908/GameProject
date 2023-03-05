using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
//Player state where they can have extra jumps after the initial jump
public class ExtraJumps : MonoBehaviour
{
    PlayerMovement playerMovement;
    BoxCollider2D collider;

    Vector2 centerRay;

    int jumpCountReset = 2;
    [SerializeField] int jumpCount;

    [SerializeField] private bool activeState = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();

        jumpCount = jumpCountReset;
    }

    private void Update()
    {
        if (!activeState) return;

        if (playerMovement.cyoteTimeCounter < 0 && Input.GetButtonDown("Jump") && jumpCount != 0)
        {
            UpdateRaycastOrigins();

            Vector2 rayOrigin = centerRay;

            RaycastHit2D centerHit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

            Vector2 dirLeft = Quaternion.Euler(0, 0, -30) * Vector2.down;
            RaycastHit2D leftHit = Physics2D.Raycast(rayOrigin, dirLeft, Mathf.Infinity);

            Vector2 dirRight = Quaternion.Euler(0, 0, 30) * Vector2.down;
            RaycastHit2D rightHit = Physics2D.Raycast(rayOrigin, dirRight, Mathf.Infinity);

            Debug.DrawRay(rayOrigin, Vector2.down * centerHit.distance, Color.green);
            Debug.DrawRay(rayOrigin, Vector2.down * leftHit.distance, Color.green);
            Debug.DrawRay(rayOrigin, Vector2.down * rightHit.distance, Color.green);

            SendMessage("Jump");
            jumpCount -= 1;
        }

        if (playerMovement.cyoteTimeCounter > 0)
        {
            jumpCount = jumpCountReset;
        }
    }

    private void ExitState(PlayerState oldState)
    {
        if (oldState == PlayerState.EXTRAJUMPS)
        {
            Debug.Log("Exit Hover Mode");
            activeState = false;
        }
    }

    private void EnterState(PlayerState newState)
    {
        if (newState == PlayerState.EXTRAJUMPS)
        {
            Debug.Log("Enter hover mode");
            activeState = true;
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        centerRay = new Vector2(bounds.center.x, bounds.min.y - 0.01f);
    }
}

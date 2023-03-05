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

    [SerializeField] int shotCount = 3;
    [SerializeField] float spread = 30f;


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

            for (int i = 0; i < shotCount; i++)
            {
                Vector2 dirLeft = Quaternion.Euler(0, 0, Random.Range(-spread, spread)) * Vector2.down;
                RaycastHit2D leftHit = Physics2D.Raycast(rayOrigin, dirLeft, Mathf.Infinity);

                Debug.DrawRay(rayOrigin, dirLeft * leftHit.distance, Random.ColorHSV(), 5f);
            }

            Debug.DrawRay(rayOrigin, Vector2.down * centerHit.distance, Color.green, 5f);



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

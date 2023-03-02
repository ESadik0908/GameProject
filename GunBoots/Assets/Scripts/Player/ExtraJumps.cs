using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class ExtraJumps : MonoBehaviour
{
    PlayerMovement playerMovement;

    int jumpCountReset = 2;
    [SerializeField] int jumpCount;

    [SerializeField] private bool activeState = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        jumpCount = jumpCountReset;
    }

    private void Update()
    {
        if (!activeState) return;

        if (playerMovement.cyoteTimeCounter < 0 && Input.GetButtonDown("Jump") && jumpCount != 0)
        {
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
}

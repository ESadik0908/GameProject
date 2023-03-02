using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class HoverJump : MonoBehaviour
{
    PlayerMovement playerMovement;

    float hoverTimeReset = 2f;
    [SerializeField] float hoverTime;

    private bool hovering;

    [SerializeField] private bool activeState = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        hoverTime = hoverTimeReset;
    }

    private void Update()
    {
        if (!activeState) return;

        if(playerMovement.cyoteTimeCounter < 0 && hoverTime > 0 && Input.GetButtonDown("Jump"))
        {
            playerMovement.gravity = -1f;
            playerMovement.velocity.y = 0;
            hovering = true;
        }
        if (Input.GetButtonUp("Jump") || hoverTime <= 0)
        {
            hovering = false;
            SendMessage("ResetGravity");
        }

        if (playerMovement.cyoteTimeCounter > 0)
        {
            hoverTime = hoverTimeReset;
        }

        if (hovering)
        {
            hoverTime -= Time.deltaTime;
        }
    }

    private void ExitState(PlayerState oldState)
    {
        if(oldState == PlayerState.HOVER)
        {
            Debug.Log("Exit Hover Mode");
            activeState = false;
        } 
    }

    private void EnterState(PlayerState newState)
    {
        if(newState == PlayerState.HOVER)
        {
            Debug.Log("Enter hover mode");
            activeState = true;
        }
    }
}

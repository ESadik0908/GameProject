using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Dash : MonoBehaviour
{
    PlayerMovement playerMovement;

    float dashBuffer = 0.2f;
    float dashBufferCounter;

    [SerializeField] int dashCountReset = 2;
    [SerializeField] int dashCount;

    [SerializeField] private bool activeState = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        dashBuffer = dashBufferCounter;
    }

    private void Update()
    {
        if (!activeState) return;

        if(dashCount > 0  && dashBufferCounter <= 0f && Input.GetKeyDown(KeyCode.LeftControl))
        {
            SendMessage("Dash");
            dashBufferCounter = dashBuffer;
            dashCount -= 1;
        }
        
        if(playerMovement.cyoteTimeCounter > 0)
        {
            dashCount = dashCountReset;
        }

        if(dashBufferCounter > 0)
        {
            dashBufferCounter -= Time.deltaTime;
        }

        
    }

    private void ExitState(PlayerState oldState)
    {
        if (oldState == PlayerState.DASH)
        {
            Debug.Log("Exit dash Mode");
            activeState = false;
        }
    }

    private void EnterState(PlayerState newState)
    {
        if (newState == PlayerState.DASH)
        {
            Debug.Log("Enter dash mode");
            activeState = true;
        }
    }
}

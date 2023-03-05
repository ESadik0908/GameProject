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
    int dashCount;

    private bool activeState = false;

    [SerializeField] float[] dashStats = new float[] {30f, 0.2f, 0.2f};

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Awake()
    {
        dashBufferCounter = 0;
    }

    private void Update()
    {
        if (!activeState) return;

        if (dashBufferCounter > 0)
        {
            dashBufferCounter -= Time.deltaTime;
        }

        if (dashCount > 0  && dashBufferCounter <= 0f && Input.GetKeyDown(KeyCode.LeftControl))
        {
            SendMessage("Dash", dashStats);
            dashCount -= 1;
            dashBufferCounter = dashBuffer;
        }
        
        if(playerMovement.cyoteTimeCounter > 0)
        {
            dashCount = dashCountReset;
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

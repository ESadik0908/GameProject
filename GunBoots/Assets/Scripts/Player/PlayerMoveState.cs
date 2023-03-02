using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    EXTRAJUMPS = 0,
    HOVER  = 1
}

public class PlayerMoveState : MonoBehaviour
{
    [SerializeField] private PlayerState startingState;

    private PlayerState currentState;

    private void Start()
    {
        EnterPlayerState(startingState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            switch (currentState)
            {
                case PlayerState.HOVER:
                    ChangeState(PlayerState.EXTRAJUMPS);
                    break;
                case PlayerState.EXTRAJUMPS:
                    ChangeState(PlayerState.HOVER);
                    break;
                default:
                    Debug.Log("Warning player is not in a valid state");
                    break;
            }
        }
    }

    private void ChangeState(PlayerState newState)
    {
        SendMessage("ExitState", currentState, SendMessageOptions.DontRequireReceiver);
        EnterPlayerState(newState);
    }

    private void EnterPlayerState( PlayerState newState)
    {
        SendMessage("EnterState", newState, SendMessageOptions.DontRequireReceiver);
        currentState = newState;
    }
}

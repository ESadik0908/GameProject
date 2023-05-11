using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    NONE = 0,
    EXTRAJUMPS = 1,
    HOVER  = 2,
    DASH = 3
}

//Controller for the players movement state, only extra jumps and hover are here but I will also add a dash
public class PlayerStateController : MonoBehaviour
{
    [SerializeField] private PlayerState startingState;

    public PlayerState currentState { get; private set; }
    
    public string GetState()
    {
        return currentState.ToString();
    }

    public void LoadState(string state)
    {
        if(state == "EXTRAJUMPS")
        {
            ShotgunUpgrade();
            return;
        }
        if (state == "HOVER")
        {
            LaserUpgrade();
            return;
        }
        if (state == "DASH")
        {
            DashUpgrade();
            return;
        }
    }

    public void ShotgunUpgrade()
    {
        ChangeState(PlayerState.EXTRAJUMPS);
    }

    public void LaserUpgrade()
    {
        ChangeState(PlayerState.HOVER);
    }

    public void DashUpgrade()
    {
        ChangeState(PlayerState.DASH);
    }
    
    private void ChangeState(PlayerState newState)
    {
        SendMessage("ExitState", currentState, SendMessageOptions.DontRequireReceiver);
        EnterPlayerState(newState);
    }

    private void EnterPlayerState(PlayerState newState)
    {
        SendMessage("EnterState", newState, SendMessageOptions.DontRequireReceiver);
        currentState = newState;
    }
}
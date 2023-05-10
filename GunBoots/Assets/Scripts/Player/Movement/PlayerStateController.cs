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

    [SerializeField] private bool extraJumpsChange = false;
    [SerializeField] private bool hoverChange = false;
    [SerializeField] private bool dashChange = false;

    Player player;

    private void Start()
    {
        FindAnyObjectByType<Player>().loadEvent += loadState;
        ChangeState(startingState);
    }

    //Change the player state when they are in a state change area and press I, eventually this will be used to change state based on current item
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && extraJumpsChange == true)
        {
            ChangeState(PlayerState.EXTRAJUMPS);
        }
        if (Input.GetKeyDown(KeyCode.E) && hoverChange == true)
        {
            ChangeState(PlayerState.HOVER);
        }
        if (Input.GetKeyDown(KeyCode.E) && dashChange == true)
        {
            ChangeState(PlayerState.DASH);
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

    public void loadState(Player player)
    {
        ChangeState(player.state);
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
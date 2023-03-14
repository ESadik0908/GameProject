using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void loadDeligate(Player p);
    public event loadDeligate loadEvent;

    private PlayerStateController playerStateController;

    public PlayerState state;

    // Start is called before the first frame update
    void Start()
    {
        playerStateController = GetComponent<PlayerStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        state = playerStateController.currentState;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        state = data.state;

        Vector3 position;

        position.x = data.playerLocation[0];
        position.y = data.playerLocation[1];
        position.z = data.playerLocation[2];
        transform.position = position;

        loadEvent(this);
    }
}

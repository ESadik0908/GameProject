using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public PlayerState state;
    public float[] playerLocation;

    public PlayerData (Player player)
    {
        state = player.state;

        playerLocation = new float[3];

        playerLocation[0] = player.transform.position.x;
        playerLocation[1] = player.transform.position.y;
        playerLocation[2] = player.transform.position.z;
    }
}

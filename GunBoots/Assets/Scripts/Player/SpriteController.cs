using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] Material material;

    private void EnterState(PlayerState newState)
    {
        if (newState == PlayerState.EXTRAJUMPS)
        {
            material.color = Color.green;
        }
        if (newState == PlayerState.HOVER)
        {
            material.color = Color.red;
        }
    }
}

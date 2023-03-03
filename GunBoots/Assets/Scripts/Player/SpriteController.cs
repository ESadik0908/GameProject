using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] Material material;

    //Change player colour based on state
    private void EnterState(PlayerState newState)
    {
        if (newState == PlayerState.EXTRAJUMPS)
        {
            material.color = new Color(1.00f, 0.30f, 0.30f);
        }
        if (newState == PlayerState.HOVER)
        {
            material.color = new Color(0.67f, 1.00f, 0.60f);
        }
        if (newState == PlayerState.DASH)
        {
            material.color = new Color(0.24f, 0.87f, 0.87f);
        }
        if (newState == PlayerState.NONE)
        {
            material.color = new Color(0.63f, 0.63f, 0.63f);
        }
    }
}

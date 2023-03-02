using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class HoverJump : MonoBehaviour
{
    Controller2D controller;
    PlayerMovement playerMovement;

    float hoverTimeReset = 2f;
    [SerializeField] float hoverTime;

    private bool hovering;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<Controller2D>();
        hoverTime = hoverTimeReset;
    }

    private void Update()
    {
        if(!controller.collisions.below && hoverTime > 0 && Input.GetButtonDown("Jump"))
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

        if (controller.collisions.below)
        {
            hoverTime = hoverTimeReset;
        }

        if (hovering)
        {
            hoverTime -= Time.deltaTime;
        }
    }
}

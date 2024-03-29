using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMovement;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float camFollowTime = 0.1f;
    private Vector3 targetPosition;
    private float yOffset = 0;
    private Vector3 target;
    private Vector3 velocity = Vector3.zero; // added for SmoothDamp

    private Vector3 playerVelocity;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        transform.position = player.transform.position + offset;
    }

    private void Update()
    {
        playerVelocity = playerMovement.getVelocity();
    }

    //Camera follows player with offset towards the players direction
    private void FixedUpdate()
    {
        camFollowTime = (Mathf.Abs(playerVelocity.x) > 8.5) ? 0.05f : 0.1f;
        offset = new Vector3((playerMovement.facing / 2), 0, -10);
        if (playerVelocity.y < -20)
        {
            offset = new Vector3(playerMovement.facing / 2, yOffset, -2);
            if (yOffset > -5)
            {
                yOffset -= Time.deltaTime * (Mathf.Abs(playerVelocity.y) / 10);
            }
        }
        if (playerMovement.coyoteTimeCounter > 0)
        {
            yOffset = 0;
        }
        target = player.transform.position + offset;

        
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, camFollowTime);
    }
}


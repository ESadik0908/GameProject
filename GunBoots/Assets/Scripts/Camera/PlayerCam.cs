using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public GameObject player;

    private PlayerMovement playerMovement;

    [SerializeField] private Vector3 offset = new Vector3(0, 0, -2);

    [SerializeField] private float camFollowTime = 0.04f;

    private Vector3 targetPosition;

    float yOffset = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        transform.position = player.transform.position + offset;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float directionY = Mathf.Sign(playerMovement.velocity.y);
        offset = new Vector3(playerMovement.facing / 2, 0, -2);
        if (playerMovement.velocity.y < -20)
        {
            Debug.Log("speed");
            offset = new Vector3(playerMovement.facing / 2, yOffset, -2);
            if(yOffset > -5)
            {
                yOffset -= Time.deltaTime * (Mathf.Abs(playerMovement.velocity.y)/10);
            }
        }
        if(playerMovement.coyoteTimeCounter > 0)
        {
            yOffset = 0;
        }
        Vector3 target = player.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, target, camFollowTime);
    }
}

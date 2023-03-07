using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    public GameObject player;

    private PlayerMovement playerMovement;

    [SerializeField] private Vector3 offset = new Vector3(0, 0, -1);
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        offset = new Vector3(playerMovement.facing, 0, -1);
        Vector3 target = player.transform.position + offset;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 oldOffset = offset;
        offset = new Vector3(playerMovement.facing, 0, -1);
        Debug.Log(offset);
        Debug.Log(oldOffset);
        Vector3 target = player.transform.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Dash : MonoBehaviour
{
    PlayerMovement playerMovement;

    [SerializeField] GameObject bombClone;
    GameObject bomb;

    private bool activeState = false;


    float dashBuffer = 0.2f;
    float dashBufferCounter;

    [SerializeField] int dashCountReset = 2;
    int dashCount;

    bool isDashing = false;

    [SerializeField] float[] dashStats = new float[] {30f, 0.2f, 0.2f};

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        bomb = Instantiate(bombClone, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));

        bomb.SetActive(false);
    }

    private void Awake()
    {
        dashBufferCounter = 0;
    }

    private void Update()
    {
        if (!activeState) return;

        if (dashBufferCounter > 0)
        {
            dashBufferCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            dashBufferCounter = dashBuffer;
        }

        if (!isDashing && dashCount > 0  && dashBufferCounter > 0f)
        {
            if (playerMovement.cyoteTimeCounter < 0)
            {
                Bomb();
            }
            

            SendMessage("Dash", dashStats);
            dashCount -= 1;
            dashBufferCounter = dashBuffer;
        }
        
        if(playerMovement.cyoteTimeCounter > 0)
        {
            dashCount = dashCountReset;
        }
    }

    IEnumerator Bomb()
    { 
        bomb.transform.position = transform.position;
        bomb.SetActive(true);
        yield return null;
    }

    private void ExitState(PlayerState oldState)
    {
        if (oldState == PlayerState.DASH)
        {
            Debug.Log("Exit dash Mode");
            activeState = false;
        }
    }

    private void EnterState(PlayerState newState)
    {
        if (newState == PlayerState.DASH)
        {
            Debug.Log("Enter dash mode");
            activeState = true;
        }
    }

    void ToggleDashing()
    {
        isDashing = !isDashing;
    }
}

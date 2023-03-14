using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerMovement))]
public class Dash : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Controller2D controller;

    [SerializeField] private GameObject bombClone;
    private GameObject[] bombs;
    private IEnumerator bombCoroutine;

    [SerializeField] private int bombCount = 8;
    [SerializeField] private float spawnDelay;

    private bool activeState = false;

    private float dashBuffer = 0.2f;
    private float dashBufferCounter;

    [SerializeField] private int dashCountReset = 2;
    private int dashCount;

    private bool isDashing = false;

    private IEnumerator dashCoroutine;

    
    [SerializeField] private float[] dashStats = new float[] {30f, 0.2f, 0.2f};//Speed, Duration, Gravity reset

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<Controller2D>();
        dashCoroutine = playerMovement.Dash(dashStats);
        bombs = new GameObject[bombCount];
        for (int i = 0; i < bombCount; i++)
        {
            bombs[i] = Instantiate(bombClone, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
            bombs[i].SetActive(false);
        }
        // Check if bombs array needs to be resized
        if (bombs.Length != bombCount)
        {
            System.Array.Resize(ref bombs, bombCount);
        }
    }

    private void Awake()
    {
        dashBufferCounter = 0;
    }

    private void Update()
    {
        if (!activeState) return;

        
        dashBufferCounter -= Time.deltaTime;
        

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            dashBufferCounter = dashBuffer;
        }

        if(!(controller.collisions.left || controller.collisions.right))
        {
            if (!isDashing && dashCount > 0 && dashBufferCounter > 0f)
            {
                bombCoroutine = Bomb();
                if (playerMovement.coyoteTimeCounter < 0)
                {
                    StartCoroutine(bombCoroutine);
                }
                StopCoroutine(dashCoroutine);
                dashCoroutine = playerMovement.Dash(dashStats);
                StartCoroutine(dashCoroutine);
                dashCount -= 1;
                dashBufferCounter = dashBuffer;
            }
        }
        

        if (controller.collisions.right || controller.collisions.left)
        { 
            if (bombCoroutine == null)
            {
                bombCoroutine = Bomb();
            } 
            StopCoroutine(bombCoroutine);
        }

        if (playerMovement.coyoteTimeCounter > 0)
        {
            dashCount = dashCountReset;
        }
    }

    private IEnumerator Bomb()
    {
        if (dashCount == 2)
        {
            for (int i = 0; i < bombCount / 2; i++)
            {
                yield return new WaitForSeconds(spawnDelay);
                Vector3 bombPosition = new Vector3(transform.position.x, transform.localPosition.y - ((transform.localScale.y / 2) + (bombs[i].transform.localScale.y) /2), transform.position.z);
                bombs[i].SetActive(true);
                bombs[i].transform.position = bombPosition; 
            }
        }
        else if (dashCount == 1)
        {
            for (int i = bombCount / 2; i < bombCount; i++)
            {
                yield return new WaitForSeconds(spawnDelay);
                Vector3 bombPosition = new Vector3 (transform.position.x, transform.localPosition.y - ((transform.localScale.y / 2) + (bombs[i].transform.localScale.y) / 2), transform.position.z);
                bombs[i].SetActive(true);
                bombs[i].transform.position = bombPosition;
            }
        }

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

    private void ToggleDashing()
    {
        isDashing = !isDashing;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerMovement))]
public class Dash : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Controller2D controller;

    private IEnumerator bombCoroutine;

    private GameObject bomb;
    [SerializeField] private GameObject bombClone;
    private static Queue<GameObject> pool = new Queue<GameObject>();

    private DashStats weponStats;

    [SerializeField] private float spawnDelay;

    private float dashBuffer = 0.2f;
    private float dashBufferCounter;

    public int dashCountReset { get; private set; }
    public int dashCount { get; private set; }

    private bool activeState = false;
    
    private bool isDashing = false;

    private IEnumerator dashCoroutine;  
    [SerializeField] private float[] dashStats = new float[] {30f, 0.2f, 0.2f};//Speed, Duration, Gravity reset
    
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<Controller2D>();
        weponStats = GetComponent<DashStats>();
        dashCoroutine = playerMovement.Dash(dashStats);
        dashCountReset = weponStats.ammo;
        
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
        for (int i = 0; i <= 4; i++)
        {
            yield return new WaitForSeconds(spawnDelay);
            bomb = GetBomb();
            bomb.transform.position = new Vector3(transform.position.x, transform.localPosition.y - ((transform.localScale.y / 2) + (bomb.transform.localScale.y) /2), transform.position.z);
            bomb.SetActive(true);
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
            dashCountReset = weponStats.ammo;
        }
    }

    private void ToggleDashing()
    {
        isDashing = !isDashing;
    }

    private GameObject GetBomb()
    {
        // check if there are any inactive bombs in the pool
        foreach (GameObject bomb in pool)
        {
            if (!bomb.activeSelf)
            {
                // if an inactive bomb is found, return it
                return bomb;
            }
        }

        // if no inactive bombs are found, create a new one
        bomb = Instantiate(bombClone);
        pool.Enqueue(bomb);
        return bomb;
    }
}

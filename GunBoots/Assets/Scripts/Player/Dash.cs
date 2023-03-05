using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Dash : MonoBehaviour
{
    PlayerMovement playerMovement;

    [SerializeField] GameObject bombClone;
    GameObject[] bombs;

    [SerializeField] int bombCount = 8;
    [SerializeField] float spawnDelay;

    private bool activeState = false;

    float dashBuffer = 0.2f;
    float dashBufferCounter;

    [SerializeField] int dashCountReset = 2;
    int dashCount;

    bool isDashing = false;

    
    [SerializeField] float[] dashStats = new float[] {30f, 0.2f, 0.2f};//Speed, Duration, Gravity reset

    void Start()
    { 
        playerMovement = GetComponent<PlayerMovement>();
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

        if (!isDashing && dashCount > 0  && dashBufferCounter > 0f)
        {
            if (playerMovement.cyoteTimeCounter < 0)
            {
                StartCoroutine(Bomb());
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
        if (dashCount == 2)
        {
            for (int i = 0; i < bombCount / 2; i++)
            {
                Vector3 bombPosition = new Vector3(transform.position.x, transform.localPosition.y - ((transform.localScale.y / 2) + (bombs[i].transform.localScale.y) /2), transform.position.z);
                bombs[i].SetActive(true);
                bombs[i].transform.position = bombPosition;
                yield return new WaitForSeconds(spawnDelay);
            }
        }
        else if (dashCount == 1)
        {
            for (int i = bombCount / 2; i < bombCount; i++)
            {
                Vector3 bombPosition = new Vector3 (transform.position.x, transform.localPosition.y - ((transform.localScale.y / 2) + (bombs[i].transform.localScale.y) / 2), transform.position.z);
                bombs[i].SetActive(true);
                bombs[i].transform.position = bombPosition;
                yield return new WaitForSeconds(spawnDelay);
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

    void ToggleDashing()
    {
        isDashing = !isDashing;
    }
}

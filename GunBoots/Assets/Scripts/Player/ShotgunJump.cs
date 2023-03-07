using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
//Player state where they can have extra jumps after the initial jump
public class ShotgunJump : MonoBehaviour
{
    PlayerMovement playerMovement;
    BoxCollider2D collider;

    Vector2 centerRay;

   

    int jumpCountReset = 2;
    [SerializeField] int jumpCount;

    [SerializeField] private bool activeState = false;

    [SerializeField] int shotCount = 4;
    [SerializeField] float spread = 30f;

    [SerializeField] GameObject bulletClone;
    GameObject[] bullets;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();

        bullets = new GameObject[shotCount];

        for(int i = 0; i < shotCount; i++)
        {
            bullets[i] = Instantiate(bulletClone, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
            bullets[i].SetActive(false);
        }
        jumpCount = jumpCountReset;
    }

    private void Update()
    {
        if (!activeState) return;

        if (playerMovement.cyoteTimeCounter < 0 && Input.GetButtonDown("Jump") && jumpCount != 0)
        {

            StartCoroutine(Shoot());

            playerMovement.Jump();
            jumpCount -= 1;
        }

        if (playerMovement.cyoteTimeCounter > 0)
        {
            jumpCount = jumpCountReset;
        }
    }

    IEnumerator Shoot()
    {
        UpdateRaycastOrigins();
        Vector2 rayOrigin = centerRay;

        for (int i = 0; i < shotCount; i++)
        {
            Vector2 curDir = i == 0 ? Vector2.down : (Vector2)(Quaternion.Euler(0, 0, Random.Range(-spread, spread)) * (Vector3)Vector2.down);

            RaycastHit2D curHit = Physics2D.Raycast(rayOrigin, curDir, Mathf.Infinity);

            Debug.DrawRay(rayOrigin, curDir * curHit.distance, Random.ColorHSV(), 5f);

            bullets[i].SetActive(true);
            Vector3 bulletSize = bullets[i].transform.localScale;
            bulletSize = bullets[i].transform.localScale;
            bullets[i].transform.position = new Vector3(centerRay.x, centerRay.y, curHit.transform.position.z);
            bullets[i].transform.localScale = new Vector3(bulletSize.x, curHit.distance + 0.1f, bulletSize.z);
            Quaternion rotation = Quaternion.FromToRotation(Vector2.down, curDir);
            bullets[i].transform.rotation = rotation;

            if (curHit.collider.gameObject.tag == "Enemy")
            {
                Debug.Log("Hit");
            }
        }

        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < shotCount + 1; i++)
        {
            bullets[i].SetActive(false);
        }
    }

    private void ExitState(PlayerState oldState)
    {
        if (oldState == PlayerState.EXTRAJUMPS)
        {
            Debug.Log("Exit Hover Mode");
            activeState = false;
        }
    }

    private void EnterState(PlayerState newState)
    {
        if (newState == PlayerState.EXTRAJUMPS)
        {
            Debug.Log("Enter hover mode");
            activeState = true;
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        centerRay = new Vector2(bounds.center.x, bounds.min.y - 0.01f);
    }
}
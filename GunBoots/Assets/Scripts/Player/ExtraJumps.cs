using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
//Player state where they can have extra jumps after the initial jump
public class ExtraJumps : MonoBehaviour
{
    PlayerMovement playerMovement;
    BoxCollider2D collider;

    Vector2 centerRay;

   

    int jumpCountReset = 2;
    [SerializeField] int jumpCount;

    [SerializeField] private bool activeState = false;

    [SerializeField] int shotCount = 3;
    [SerializeField] float spread = 30f;
    [SerializeField] GameObject bulletClone;

    GameObject[] bullets;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();

        bullets = new GameObject[shotCount + 1];

        for(int i = 0; i < shotCount + 1; i++)
        {
            bullets[i] = Instantiate(bulletClone, new Vector3 (transform.position.x + (i * 5), transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
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

            SendMessage("Jump");
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

        RaycastHit2D centerHit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

        bullets[0].SetActive(true);

        Vector3 bulletSize = bullets[0].transform.localScale;

        bullets[0].transform.position = new Vector3(rayOrigin.x, rayOrigin.y, centerHit.transform.position.z);

        bullets[0].transform.localScale = new Vector3(bulletSize.x, centerHit.distance, bulletSize.z);

        for (int i = 0; i < shotCount; i++)
        {
            Vector2 curDir = Quaternion.Euler(0, 0, Random.Range(-spread, spread)) * Vector2.down;
            RaycastHit2D curHit = Physics2D.Raycast(rayOrigin, curDir, Mathf.Infinity);

            Debug.DrawRay(rayOrigin, curDir * curHit.distance, Random.ColorHSV(), 5f);

            bullets[i + 1].SetActive(true);

            bulletSize = bullets[i + 1].transform.localScale;

            bullets[i + 1].transform.position = new Vector3(centerRay.x, centerRay.y, curHit.transform.position.z);

            bullets[i + 1].transform.localScale = new Vector3(bulletSize.x, curHit.distance, bulletSize.z);

            Quaternion rotation = Quaternion.FromToRotation(Vector2.down, curDir);

            bullets[i + 1].transform.rotation = rotation;
        }

        Debug.DrawRay(rayOrigin, Vector2.down * centerHit.distance, Color.green, 5f);

        yield return new WaitForSeconds(0.1f);

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
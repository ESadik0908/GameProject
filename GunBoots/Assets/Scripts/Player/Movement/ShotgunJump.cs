using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
//Player state where they can have extra jumps after the initial jump
public class ShotgunJump : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private BoxCollider2D collider;

    private Vector2 centerRay;

    public int jumpCountReset { get; private set; }
    public int jumpCount { get; private set; }

    private bool activeState = false;

    [SerializeField] private int shotCount = 5;
    [SerializeField] private float spread = 30f;

    [SerializeField] private GameObject bulletClone;
    private GameObject[] bullets;

    private ShotgunStats weponStats;



    private void Start()
    {

        playerMovement = GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();
        weponStats = GetComponent<ShotgunStats>();
        jumpCountReset = weponStats.ammo;
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

        if (PauseMenu.GameIsPaused || UpgradeMenu.GameIsPaused || GameOverMenu.GameIsPaused || TimeBody.isRewinding) return;

        jumpCountReset = weponStats.ammo;

        if (playerMovement.coyoteTimeCounter < 0 && Input.GetButtonDown("Jump") && jumpCount != 0) // Coyote timer is > 0 when player is grounded
        {

            StartCoroutine(Shoot());
            playerMovement.jump = true; 
            jumpCount -= 1;
        }

        if (playerMovement.coyoteTimeCounter > 0)
        {
            jumpCount = jumpCountReset;
        }
    }

    //Generate shotCount random rays from the players feet to the closest hit object. The first ray is always straight down, the remaining rays
    //are in a random angle between -spread and spread. Activate each bullet to one of the rays for 0.07s and then deactivate them.
    private IEnumerator Shoot()
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
                GameObject enemy = curHit.collider.gameObject;
                if (enemy.TryGetComponent<EnemyStatController>(out EnemyStatController enemyHealth))
                {
                    enemyHealth.Damage(weponStats.damage);
                }
            }
        }

        yield return new WaitForSeconds(0.07f);

        for (int i = 0; i < shotCount; i++)
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
            jumpCountReset = weponStats.ammo;
        }
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        centerRay = new Vector2(bounds.center.x, bounds.min.y - 0.01f);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(GameObject))]
//The player can hover after jumping by holding down spacebar/jump key
public class LaserJump : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private BoxCollider2D collider;
    [SerializeField] private GameObject laserPrefab;
    private GameObject laser;

    public float hoverTimeReset { get; private set; }
    public float hoverTime { get; private set; }

    private Vector2 centerRay;

    private bool hovering;

    [SerializeField] private bool activeState = false;

    private LaserStats weponStats;

    [SerializeField] private float damageBufferReset = 0.1f;
    private float damageBuffer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();
        weponStats = GetComponent<LaserStats>();
        hoverTimeReset = weponStats.ammo;
        hoverTime = hoverTimeReset;
        laser = Instantiate(laserPrefab);
        damageBuffer = damageBufferReset;
    }

    private void OnEnable()
    {
        playerMovement = GetComponent<PlayerMovement>();
        collider = GetComponent<BoxCollider2D>();
        weponStats = GetComponent<LaserStats>();
        hoverTimeReset = weponStats.ammo;
        hoverTime = hoverTimeReset;
        laser = Instantiate(laserPrefab);
        damageBuffer = damageBufferReset;
    }

    private void Update()
    {
        damageBuffer -= Time.deltaTime;
        if (!activeState)
        {
            laser.SetActive(false);
            return;
        }
        UpdateRaycastOrigins();
        if (playerMovement.coyoteTimeCounter < 0 && hoverTime > 0 && Input.GetButtonDown("Jump"))
        {
            playerMovement.Hover();
            hovering = true;
        }
        if (Input.GetButtonUp("Jump") || hoverTime <= 0)
        {
            hovering = false;

            playerMovement.ResetGravity();
        }

        if (playerMovement.coyoteTimeCounter > 0)
        {
            hoverTime = hoverTimeReset;
        }

        if (hovering)
        {
            laser.SetActive(true);
            Vector2 rayOrigin = centerRay;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity);

            if (hit.collider != null)
            {
                // Draw the ray up to the point where it hits an object
                Debug.DrawRay(rayOrigin, Vector2.down * hit.distance, Color.green);

                if (hit.collider.gameObject.tag == "Enemy")
                {
                    if(damageBuffer <= 0)
                    {
                        GameObject enemy = hit.collider.gameObject;
                        if (enemy.TryGetComponent(out EnemyStatController enemyHealth))
                        {
                            enemyHealth.Damage(weponStats.damage);
                            damageBuffer = damageBufferReset;
                        }

                        if (enemy.TryGetComponent(out EnemyProjectileStats flyingEnemyStats))
                        {
                            flyingEnemyStats.Damage(weponStats.damage);
                            damageBuffer = damageBufferReset;
                        }
                    }
                }
                
                Vector3 laserSize = laser.transform.localScale;

                laser.transform.position = new Vector3 (rayOrigin.x ,rayOrigin.y-(hit.distance /2), laser.transform.position.z);

                laser.transform.localScale = new Vector3(laserSize.x, hit.distance + 0.1f, laserSize.z);
            }
            else
            {
                // Draw the full length of the ray
                Debug.DrawRay(rayOrigin, -Vector2.up * 20f, Color.green);
            }
            if(hoverTime != 0)
            {
                hoverTime -= Time.deltaTime;
            }
            if(hoverTime < 0)
            {
                hoverTime = 0;
            }
        }
        else
        {
            laser.SetActive(false);
        }
    }

    private void ExitState(PlayerState oldState)
    {
        if(oldState == PlayerState.HOVER)
        {
            Debug.Log("Exit Hover Mode");
            activeState = false;
        } 
    }

    private void EnterState(PlayerState newState)
    {
        if(newState == PlayerState.HOVER)
        {
            Debug.Log("Enter hover mode");
            activeState = true;
        }
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        centerRay = new Vector2(bounds.center.x, bounds.min.y - 0.01f);
    }
}

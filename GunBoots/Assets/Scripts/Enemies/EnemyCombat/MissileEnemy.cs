using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileEnemy : MonoBehaviour
{

    private Controller2D controller;
    private PlayerMovement playerMovement;
    private float gravity;
    private Vector3 velocity;
    private GameObject player;


    [SerializeField] private float minGunCooldown;
    [SerializeField] private float maxGunCooldown;
    private float gunCooldown = 3f;

    [SerializeField] private GameObject missileClone;
    private GameObject missile;
    private static Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        controller = GetComponent<Controller2D>();
        gravity = playerMovement.getGravity();
        pool.Clear();
    }

    private void OnEnable()
    {
        pool.Clear();
    }

    private void Update()
    {
        if (TimeBody.isRewinding) return;
        if (velocity.y > -50)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (gunCooldown <= 0)
        {
            Shoot();
            gunCooldown = Random.Range(minGunCooldown, maxGunCooldown);
            return;
        }

        gunCooldown -= Time.deltaTime;


    }

    private void FixedUpdate()
    {
        if (TimeBody.isRewinding) return;
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void Shoot()
    {
        missile = GetMissile();
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2) + 0.1f, transform.position.z);
        missile.transform.up = transform.up;
        missile.transform.position = spawnPos;
        
        missile.SetActive(true);
    }

    private GameObject GetMissile()
    {
        // check if there are any inactive bombs in the pool
        foreach (GameObject missile in pool)
        {
            if (!missile.activeSelf)
            {
                // if an inactive bomb is found, return it
                return missile;
            }
        }

        // if no inactive bombs are found, create a new one
        missile = Instantiate(missileClone);
        pool.Enqueue(missile);
        return missile;
    }
}

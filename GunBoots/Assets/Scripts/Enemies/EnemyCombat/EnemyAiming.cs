using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiming : MonoBehaviour
{
    public Transform target;

    [SerializeField] private GameObject bulletClone;
    private GameObject bullet;
    private static Queue<GameObject> pool = new Queue<GameObject>();

    private float gunCooldown;
    [SerializeField] private float gunCooldownReset;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        gunCooldown = gunCooldownReset;
    }

    private void Update()
    {
        if(gunCooldown <= 0)
        {
            Fire();
            gunCooldown = gunCooldownReset;
            return;
        }

        gunCooldown -= Time.deltaTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z; // Set the target's z position to be the same as the enemy's

        transform.up = targetPosition - transform.position;
        
    }

    private void Fire()
    {
        bullet = GetBullet();
        bullet.transform.position = transform.parent.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);
    }

    private GameObject GetBullet()
    {
        // check if there are any inactive bombs in the pool
        foreach (GameObject bullet in pool)
        {
            if (!bullet.activeSelf)
            {
                // if an inactive bomb is found, return it
                return bullet;
            }
        }

        // if no inactive bombs are found, create a new one
        bullet = Instantiate(bulletClone);
        pool.Enqueue(bullet);
        return bullet;
    }
}


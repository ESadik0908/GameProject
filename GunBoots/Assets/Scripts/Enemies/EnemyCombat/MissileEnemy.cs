using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileEnemy : MonoBehaviour
{

    [SerializeField] private float minGunCooldown;
    [SerializeField] private float maxGunCooldown;
    private float gunCooldown = 3f;

    [SerializeField] private GameObject missileClone;
    private GameObject missile;
    private static Queue<GameObject> pool = new Queue<GameObject>();

    private void Update()
    {
        if(gunCooldown <= 0)
        {
            Shoot();
            gunCooldown = Random.RandomRange(minGunCooldown, maxGunCooldown);
            return;
        }

        gunCooldown -= Time.deltaTime;
    }

    private void Shoot()
    {
        missile = GetMissile();
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2) + 0.1f, transform.position.z);
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

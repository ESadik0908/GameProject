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

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        gunCooldown = gunCooldownReset;
        pool.Clear();
    }

    private void OnEnable()
    {
        pool.Clear();
    }

    private void Update()
    {
        if (TimeBody.isRewinding) return;
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance > 30)
        {
            return;
        }

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
        if (TimeBody.isRewinding) return;
        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z;

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
        foreach (GameObject bullet in pool)
        {
            if (!bullet.activeSelf)
            {
                return bullet;
            }
        }

        bullet = Instantiate(bulletClone);
        pool.Enqueue(bullet);
        return bullet;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiming : MonoBehaviour
{
    public Transform target;
    [SerializeField] private GameObject bullet;

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
        Instantiate(bullet, transform.parent.position, transform.rotation);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatController : MonoBehaviour, IEnemyStats
{
    public float health { get; private set; }

    [SerializeField] private float tmp;

    [SerializeField] private float _contactDamage;

    public float contactDamage
    {
        get
        {
            return _contactDamage;
        }
    }

    private float damageBuffer = 0.1f;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    private void Start()
    {
        health = tmp;
    }

    private void Update()
    {
        tmp = health;
        if(damageBuffer > 0)
        {
            damageBuffer -= Time.deltaTime;
        }
    }

    public void Damage(float damage)
    {
        if (damageBuffer < 0)
        {
            health -= damage;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}

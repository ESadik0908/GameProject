using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IEnemyStats
{
    public float health { get; private set; }

    [SerializeField] private float tmp;

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

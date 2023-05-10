using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileStats : MonoBehaviour, IEnemyStats
{
    public float health { get; private set; }

    [SerializeField] private float maxHealth;

    [SerializeField] private float tmp;

    [SerializeField] private int _contactDamage;

    private GameObject gameTracker;
    private GameStatsTracker gameStatsTrackerScript;

    public int contactDamage
    {
        get
        {
            return _contactDamage;
        }
    }

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    private void Start()
    {
        gameTracker = GameObject.FindGameObjectWithTag("GameController");
        gameStatsTrackerScript = gameTracker.GetComponent<GameStatsTracker>();
        health = maxHealth;
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    private void Update()
    {
        tmp = health;
        
    }

    public void Damage(float damage)
    {
        
        health -= damage;

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

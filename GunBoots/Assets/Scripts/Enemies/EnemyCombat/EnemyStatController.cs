using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatController : MonoBehaviour, IEnemyStats
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "PlayerBullet")
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            if (player.TryGetComponent(out PlayerHealthController playerHealth))
            {
                playerHealth.Damage(contactDamage);
                Debug.Log(contactDamage);
            }
        }
    }

    public void Die()
    {
        transform.position = new Vector3(-50000, -50000);
        gameStatsTrackerScript.EnemyDied();
        gameObject.SetActive(false);
    }
}

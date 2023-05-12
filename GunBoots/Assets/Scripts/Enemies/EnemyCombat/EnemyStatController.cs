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


    private EnemyTimeBody unitTimeBody;


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
        unitTimeBody = GetComponent<EnemyTimeBody>();
        health = maxHealth;
    }

    private void OnEnable()
    {
        health = maxHealth;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "PlayerBullet")
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            float side = Mathf.Sign(player.transform.position.x - transform.position.x);
            if (player.TryGetComponent(out PlayerHealthController playerHealth))
            {
                playerHealth.Damage(contactDamage);
            }

            if(player.TryGetComponent(out PlayerMovement playerMovement))
            {
                playerMovement.KnockBack((contactDamage*2) * side);
            }
        }
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
        unitTimeBody.ResetHistory(true);
        transform.position = new Vector3(-50000, -50000);
        gameStatsTrackerScript.EnemyDied();
        gameObject.SetActive(false);
    }
}

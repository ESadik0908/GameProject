using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public float health { get; private set; }
    public float maxHealth;

    [SerializeField] private float damageBuffer;
    private float damageBufferCounter;

    private void Start()
    {
        health = maxHealth;
        damageBufferCounter = damageBuffer;
    }

    private void Update()
    {
        if(damageBufferCounter > 0)
        {
            damageBufferCounter -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject enemy = collision.gameObject;
            if (enemy.TryGetComponent<EnemyStatController>(out EnemyStatController enemyStat))
            {
                if (damageBufferCounter <= 0 && health > 0)
                {
                    health -= enemyStat.contactDamage;
                    damageBufferCounter = damageBuffer;
                }
            }
        }
    }
}

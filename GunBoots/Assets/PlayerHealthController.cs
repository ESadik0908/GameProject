using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public float health { get; private set; }
    public float maxHealth;
    public int extraLives = 1;

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

    public void Damage(int damage)
    {
        if (damageBufferCounter <= 0 && health > 0)
        {
            health -= damage;
            damageBufferCounter = damageBuffer;
        }

        if(health <= 0)
        {
            if(extraLives == 0)
            {
                gameObject.SetActive(false);
                //Game Over Screen
            }
            extraLives -= 1;
            health = maxHealth;
        }
    }
}

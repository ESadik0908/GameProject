using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public float health { get; private set; }
    [SerializeField] private float defaultHealth = 100;
    public float maxHealth;
    public int extraLives = 1;

    [SerializeField] private float damageBuffer;
    private float damageBufferCounter;

    private PlayerUpgrades playerUpgrades;

    private void Start()
    {
        maxHealth = defaultHealth;
        playerUpgrades = GetComponent<PlayerUpgrades>();
        health = maxHealth;
        damageBufferCounter = damageBuffer;
    }

    private void Update()
    {
        if(damageBufferCounter > 0)
        {
            damageBufferCounter -= Time.deltaTime;
        }
        float oldHealth = maxHealth;
        maxHealth = defaultHealth + (playerUpgrades.health * 10);
        if(oldHealth != maxHealth)
        {
            health += (playerUpgrades.health * 10);
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

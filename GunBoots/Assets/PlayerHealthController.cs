using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealthController : MonoBehaviour
{
    public event Action<bool> PlayerHasDied;

    public float health { get; private set; }
    [SerializeField] private float defaultHealth = 100;
    public float maxHealth;
    public int extraLives = 1;
    private TimeBody timeBody;

    [SerializeField] private float damageBuffer;
    private float damageBufferCounter;

    private PlayerUpgrades playerUpgrades;

    private void Start()
    {
        timeBody = GetComponent<TimeBody>();
        maxHealth = defaultHealth;
        playerUpgrades = GetComponent<PlayerUpgrades>();
        playerUpgrades.OnHealthUpgrade += HandleHealthUpgrade;
        playerUpgrades.OnUpgradeLives += HandleLivesUpgrade;
        playerUpgrades.OnTakeUpgrade += Heal;
        damageBufferCounter = damageBuffer;
    }
    
    private void Update()
    {
        if (damageBufferCounter > 0)
        {
            damageBufferCounter -= Time.deltaTime;
        }
    }

    public void Heal(float healAmmount)
    {
        if(health + healAmmount > maxHealth)
        {
            health = maxHealth;
            return;
        }

        health += healAmmount;
    }

    public void Damage(int damage)
    {
        if (TimeBody.isRewinding) return;
        if (damageBufferCounter <= 0 && health > 0)
        {
            health -= damage;
            damageBufferCounter = damageBuffer;
        }

        if (health <= 0)
        {
            if (extraLives == 0)
            {
                StartCoroutine("Despawn");
            }
            else
            {
                timeBody.StartRewind();
            }
            extraLives -= 1;
            health = maxHealth;
        }
    }

    public void LoadHealth(float _curHealth, float _maxHealth, int _extraLives)
    {
        maxHealth = _maxHealth;
        health = _curHealth;
        extraLives = _extraLives;
    }

    private void HandleHealthUpgrade(int healthIncrease)
    {
        maxHealth += healthIncrease;
        health += healthIncrease;
    }

    private void HandleLivesUpgrade(int livesIncrease)
    {
        extraLives += livesIncrease;
    }

    public bool HasSavedData()
    {
        return PlayerPrefs.HasKey("PlayerCurrentHealth");
    }

    private IEnumerator Despawn()
    {
        PlayerHasDied?.Invoke(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }
}

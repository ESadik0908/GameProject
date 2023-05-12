using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealthController : MonoBehaviour
{
    public event Action<bool> PlayerHasDied;
    public static event Action<string> NoDamage;

    public float health { get; private set; }
    [SerializeField] private float defaultHealth = 100;
    public float maxHealth;
    public int extraLives = 1;
    private TimeBody timeBody;
    
    [SerializeField] private float damageBuffer;

    private bool canTakeDamage = true;

    private PlayerUpgrades playerUpgrades;

    private int lastWaveDamaged;

    [SerializeField] Material material;

    private void Start()
    {
        timeBody = GetComponent<TimeBody>();
        maxHealth = defaultHealth;
        playerUpgrades = GetComponent<PlayerUpgrades>();
        playerUpgrades.OnHealthUpgrade += HandleHealthUpgrade;
        playerUpgrades.OnUpgradeLives += HandleLivesUpgrade;
        playerUpgrades.OnTakeUpgrade += Heal;
        lastWaveDamaged = GameStatsTracker.wave;
    }
    
    private void Update()
    {
        if(GameStatsTracker.wave - lastWaveDamaged >= 5)
        {
            NoDamage?.Invoke("UNTOUCHABLE");
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
        if (canTakeDamage && health > 0)
        {
            lastWaveDamaged = GameStatsTracker.wave;
            health -= damage;
            StartCoroutine(Invunrable());
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

    private IEnumerator Invunrable()
    {
        canTakeDamage = false;

        float timeElapsed = 0f;
        float interval = damageBuffer / 10;
        int index = 0;

        while(index < 10)
        {
            Color colourBlock1 = material.color;
            colourBlock1.a = index % 2;
            material.color = colourBlock1;
            timeElapsed += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(interval);
        }

        Color colourBlock = material.color;
        colourBlock.a = 1;
        material.color = colourBlock;

        canTakeDamage = true;
    }




    private IEnumerator Despawn()
    {
        PlayerHasDied?.Invoke(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }
}

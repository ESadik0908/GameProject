using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUpgrades : MonoBehaviour
{
    public event Action<int> OnHealthUpgrade;
    public event Action<int> OnUpgradeLives;
    public event Action<float> OnTakeUpgrade;

    public int ammo;
    public int damage;
    public int speed;
    public int health;
    public int lives;

    public void UpgradeAmmo()
    {
        ammo += 1;
        OnTakeUpgrade?.Invoke(50);
    }

    public void UpgradeSpeed()
    {
        speed += 1;
        OnTakeUpgrade?.Invoke(50);
    }

    public void UpgradeHealth()
    {
        health += 1;
        OnHealthUpgrade?.Invoke(10);
        OnTakeUpgrade?.Invoke(50);
    }

    public void UpgradeDamage()
    {
        damage += 1;
        OnTakeUpgrade?.Invoke(50);
    }

    public void UpgradeLives()
    {
        lives += 1;
        OnUpgradeLives?.Invoke(1);
        OnTakeUpgrade?.Invoke(50);
    }

    public void LoadUpgrades(int _ammo, int _damage, int _speed, int _health, int _lives)
    {
        ammo = _ammo;
        damage = _damage;
        speed = _speed;
        health = _health;
        lives = _lives;
    }
}

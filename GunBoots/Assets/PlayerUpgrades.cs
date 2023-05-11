using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUpgrades : MonoBehaviour
{
    public event Action<int> OnHealthUpgrade;

    public int ammo;
    public int damage;
    public int speed;
    public int health;

    public void UpgradeAmmo()
    {
        ammo += 1;
    }

    public void UpgradeSpeed()
    {
        speed += 1;
    }

    public void UpgradeHealth()
    {
        health += 1;
        OnHealthUpgrade?.Invoke(10);
    }

    public void UpgradeDamage()
    {
        damage += 1;
    }

    public void LoadUpgrades(int _ammo, int _damage, int _speed, int _health)
    {
        Debug.Log("load");
        ammo = _ammo;
        damage = _damage;
        speed = _speed;
        health = _health;
    }
}

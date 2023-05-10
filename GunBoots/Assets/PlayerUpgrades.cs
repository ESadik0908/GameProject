using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
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
    }

    public void UpgradeDamage()
    {
        damage += 1;
    }
}

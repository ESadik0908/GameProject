using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public int ammo { get; private set; }
    public int damage { get; private set; }
    public int speed { get; private set; }
    public int health { get; private set; }

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
        health += 10;
    }

    public void UpgradeDamage()
    {
        damage += 10;
    }
}

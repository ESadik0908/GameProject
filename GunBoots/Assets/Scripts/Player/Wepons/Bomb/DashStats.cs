using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStats : MonoBehaviour
{
    [SerializeField] private int initDamage;
    [SerializeField] private int initAmmo;
    private bool activeState = false;
    private PlayerUpgrades playerUpgrades;

    private void Start()
    {
        playerUpgrades = GetComponent<PlayerUpgrades>();
        damage = initDamage;
        ammo = initAmmo;
    }

    private void Update()
    {
        if (!activeState)
        {
            return;
        }
        damage = initDamage + (playerUpgrades.damage * 10);
        ammo = initAmmo + playerUpgrades.ammo;
        tempDamage = damage;
        tempAmmo = ammo;
    }

    public int damage { get; private set; }
    public int ammo { get; private set; }

    [SerializeField] private int tempAmmo;
    [SerializeField] private int tempDamage;

    private void ExitState(PlayerState oldState)
    {
        if (oldState == PlayerState.DASH)
        {
            Debug.Log("Exit dash Mode");
            activeState = false;
        }
    }

    private void EnterState(PlayerState newState)
    {
        if (newState == PlayerState.DASH)
        {
            Debug.Log("Enter dash mode");
            activeState = true;
        }
    }

}

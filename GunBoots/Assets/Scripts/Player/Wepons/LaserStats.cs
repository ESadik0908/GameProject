using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStats : MonoBehaviour
{
    [SerializeField] private int initDamage;
    [SerializeField] private float initAmmo;
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
        damage = initDamage + (playerUpgrades.damage * 2);
        ammo = initAmmo + playerUpgrades.ammo;
        tempDamage = damage;
        tempAmmo = ammo;
    }

    public int damage { get; private set; }
    public float ammo { get; private set; }

    [SerializeField] private float tempAmmo;
    [SerializeField] private float tempDamage;

    private void ExitState(PlayerState oldState)
    {
        if (oldState == PlayerState.HOVER)
        {
            Debug.Log("Exit dash Mode");
            activeState = false;
        }
    }

    private void EnterState(PlayerState newState)
    {
        if (newState == PlayerState.HOVER)
        {
            Debug.Log("Enter dash mode");
            activeState = true;
        }
    }
}

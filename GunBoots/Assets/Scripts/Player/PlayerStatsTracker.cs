using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    private Dash dash;
    private ShotgunJump shotgun;
    private LaserJump laser;
    private PlayerHealthController playerHealth;

    public PlayerStateController stateController { get; private set; }

    public float ammo;
    public float maxAmmo;
    public float health;
    public float maxHealth;

    private void Start()
    {
        dash = GetComponent<Dash>();
        shotgun = GetComponent<ShotgunJump>();
        laser = GetComponent<LaserJump>();
        playerHealth = GetComponent<PlayerHealthController>();
        stateController = GetComponent<PlayerStateController>();
    }

    private void Update()
    {
        maxHealth = playerHealth.maxHealth;
        health = playerHealth.health;
        GetAmmoCount();
    }

    private void GetAmmoCount()
    {
        if (stateController.currentState == PlayerState.DASH)
        {
            ammo = dash.dashCount;
            maxAmmo = dash.dashCountReset;
            return;
        }

        if (stateController.currentState == PlayerState.EXTRAJUMPS)
        {
            ammo = shotgun.jumpCount;
            maxAmmo = shotgun.jumpCountReset;
            return;
        }

        if (stateController.currentState == PlayerState.HOVER)
        {
            ammo = laser.hoverTime;
            maxAmmo = laser.hoverTimeReset;
            return;
        }
    }
}

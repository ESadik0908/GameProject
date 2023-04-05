using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    private Dash dash;
    private ShotgunJump shotgun;
    private LaserJump laser;

    public PlayerStateController stateController { get; private set; }

    public float ammo;
    public float maxAmmo;

    private void Start()
    {
        dash = GetComponent<Dash>();
        shotgun = GetComponent<ShotgunJump>();
        laser = GetComponent<LaserJump>();
        stateController = GetComponent<PlayerStateController>();
    }

    private void Update()
    {
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

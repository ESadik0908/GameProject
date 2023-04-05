using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject laserAmmo;
    private Slider ammoCountSlider;

    [SerializeField] private GameObject shotgunAmmo;
    private Text shotgunAmmoCount;

    [SerializeField] private GameObject bombAmmo;
    private Text bombAmmoCount;

    private GameObject player;
    private PlayerStatsTracker playerStats;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStatsTracker>();
        ammoCountSlider = laserAmmo.GetComponentInChildren<Slider>();

        shotgunAmmoCount = shotgunAmmo.GetComponentInChildren<Text>();

        bombAmmoCount = bombAmmo.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.stateController.currentState == PlayerState.HOVER)
        {
            laserAmmo.SetActive(true);
            ammoCountSlider.maxValue = playerStats.maxAmmo;
            ammoCountSlider.value = playerStats.ammo;
        }
        else
        {
            laserAmmo.SetActive(false);
        }

        if (playerStats.stateController.currentState == PlayerState.EXTRAJUMPS)
        {
            shotgunAmmo.SetActive(true);
            shotgunAmmoCount.text = ("X " + playerStats.ammo.ToString());
        }
        else
        {
            shotgunAmmo.SetActive(false);
        }

        if (playerStats.stateController.currentState == PlayerState.DASH)
        {
            bombAmmo.SetActive(true);
            bombAmmoCount.text = ("X " + playerStats.ammo.ToString());
        }
        else
        {
            bombAmmo.SetActive(false);
        }


    }
}

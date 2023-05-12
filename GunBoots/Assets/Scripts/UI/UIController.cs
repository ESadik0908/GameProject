using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject laserAmmo;
    private Slider ammoCountSlider;

    [SerializeField] private GameObject shotgunAmmo;
    private TMP_Text shotgunAmmoCount;

    [SerializeField] private GameObject bombAmmo;
    private TMP_Text bombAmmoCount;

    [SerializeField] private GameObject playerHealth;
    private Slider playerHealthSlider;
    [SerializeField] private Gradient healthBarGradient;
    private Image healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text playerLives;

    [SerializeField] private TMP_Text waveCounter;
    [SerializeField] private TMP_Text enemiesRemaining;

    [SerializeField] private GameObject gameController;
    private GameStatsTracker gameStats;

    [SerializeField] TMP_Text upgradesText;

    private GameObject player;
    private PlayerStatsTracker playerStats;
    private PlayerUpgrades playerUpgrades;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerUpgrades = player.GetComponent<PlayerUpgrades>();
        playerStats = player.GetComponent<PlayerStatsTracker>();
        ammoCountSlider = laserAmmo.GetComponentInChildren<Slider>();

        gameStats = gameController.GetComponent<GameStatsTracker>();

        shotgunAmmoCount = shotgunAmmo.GetComponentInChildren<TMP_Text>();

        bombAmmoCount = bombAmmo.GetComponentInChildren<TMP_Text>();

        playerHealthSlider = playerHealth.GetComponentInChildren<Slider>();
        healthBar = playerHealth.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        updateUpgrades();
        UpdateHealth();
        UpdateAmmo();
        UpdateProgress();
    }

    private void updateUpgrades()
    {
        upgradesText.text = (
            "Upgrades: \n"
            + "Damage: " + playerUpgrades.damage + "\n"
            + "Ammo: " + playerUpgrades.ammo + "\n"
            + "Speed: " + playerUpgrades.speed + "\n"
            + "Health: " + playerUpgrades.health + "\n"
        );
    }

    private void UpdateAmmo()
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

    private void UpdateHealth()
    {
        playerHealthSlider.maxValue = playerStats.maxHealth;
        playerHealthSlider.value = playerStats.health;
        playerLives.text = playerStats.lives.ToString();

        healthBar.color = healthBarGradient.Evaluate(playerHealthSlider.normalizedValue);

        healthText.text = (playerHealthSlider.value.ToString() + " / " + playerHealthSlider.maxValue.ToString());
    }

    private void UpdateProgress()
    {
        waveCounter.text = ("Wave: " + GameStatsTracker.wave.ToString());
        enemiesRemaining.text = ("Enemies Remaining: " + gameStats.enemiesRemaining.ToString());
    }
}

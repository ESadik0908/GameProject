using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private GameObject player;
    private GameObject gameController;

    private PlayerStateController stateController;
    private PlayerHealthController playerHealth;
    private PlayerUpgrades playerUpgrades;

    private GameStatsTracker gameStats;

    public static float defaultMaxHealth = 100;
    public static int defaultExtraLives = 1;

    public static string defaultPlayerState = "NONE";

    private string profile;

    public static int defaultLivesUpgrades = 0;
    public static int defaultDamageUpgrades = 0;
    public static int defaultHealthUpgrades = 0;
    public static int defaultAmmoUpgrades = 0;
    public static int defaultSpeedUpgrades = 0;

    public static int defaultWave = 1;
    
    private void Start()
    {
        profile = PlayerPrefs.GetString("Profile");
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController");

        stateController = player.GetComponent<PlayerStateController>();
        playerHealth = player.GetComponent<PlayerHealthController>();
        playerUpgrades = player.GetComponent<PlayerUpgrades>();

        playerHealth.PlayerHasDied += DeleteSavedData;

        gameStats = gameController.GetComponent<GameStatsTracker>();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(profile + "NewRun", 1);
        PlayerPrefs.SetString(profile + "PlayerState", stateController.GetState());

        PlayerPrefs.SetFloat(profile + "PlayerCurrentHealth", playerHealth.health);
        PlayerPrefs.SetFloat(profile + "PlayerMaxHealth", playerHealth.maxHealth);
        PlayerPrefs.SetInt(profile + "PlayerExtraLives", playerHealth.extraLives);

        PlayerPrefs.SetInt(profile + "PlayerDamageUpgrades", playerUpgrades.damage);        
        PlayerPrefs.SetInt(profile + "PlayerHealthUpgrades", playerUpgrades.health);
        PlayerPrefs.SetInt(profile + "PlayerAmmoUpgrades", playerUpgrades.ammo);
        PlayerPrefs.SetInt(profile + "PlayerSpeedUpgrades", playerUpgrades.speed);
        PlayerPrefs.SetInt(profile + "PlayerLivesUpgrades", playerUpgrades.lives);

        // Save game controller data
        PlayerPrefs.SetInt(profile + "WaveCount", GameStatsTracker.wave);

        // Save the PlayerPrefs to disk
        PlayerPrefs.Save();
    }

    public void Load()
    {
        // Retrieve player data
        string playerState = PlayerPrefs.GetString(profile + "PlayerState");

        float currentHealth = PlayerPrefs.GetFloat(profile + "PlayerCurrentHealth");
        float maxHealth = PlayerPrefs.GetFloat(profile + "PlayerMaxHealth");
        int extraLives = PlayerPrefs.GetInt(profile + "PlayerExtraLives");

        int damageUpgrades = PlayerPrefs.GetInt(profile + "PlayerDamageUpgrades");
        int healthUpgrades = PlayerPrefs.GetInt(profile + "PlayerHealthUpgrades");
        int ammoUpgrades = PlayerPrefs.GetInt(profile + "PlayerAmmoUpgrades");
        int speedUpgrades = PlayerPrefs.GetInt(profile + "PlayerSpeedUpgrades");
        int livesUpgrades = PlayerPrefs.GetInt(profile + "PlayerLivesUpgrades");

        // Retrieve game controller data
        int waveCount = PlayerPrefs.GetInt(profile + "WaveCount");

        // Load the data into the appropriate scripts
        stateController.LoadState(playerState);
        playerHealth.LoadHealth(currentHealth, maxHealth, extraLives);
        playerUpgrades.LoadUpgrades(ammoUpgrades, damageUpgrades, speedUpgrades, healthUpgrades, livesUpgrades);
        gameStats.LoadGame(waveCount);
    }
    
    public void DeleteSavedData(bool reset)
    {
        PlayerPrefs.SetInt(profile + "NewRun", 0);
        PlayerPrefs.SetString(profile + "PlayerState", defaultPlayerState);

        PlayerPrefs.SetFloat(profile + "PlayerCurrentHealth", defaultMaxHealth);
        PlayerPrefs.SetFloat(profile + "PlayerMaxHealth", defaultMaxHealth);
        PlayerPrefs.SetInt(profile + "PlayerExtraLives", defaultExtraLives);

        PlayerPrefs.SetInt(profile + "PlayerDamageUpgrades", defaultDamageUpgrades);
        PlayerPrefs.SetInt(profile + "PlayerHealthUpgrades", defaultHealthUpgrades);
        PlayerPrefs.SetInt(profile + "PlayerAmmoUpgrades", defaultAmmoUpgrades);
        PlayerPrefs.SetInt(profile + "PlayerSpeedUpgrades", defaultSpeedUpgrades);
        PlayerPrefs.SetInt(profile + "PlayerLivesUpgrades", defaultLivesUpgrades);

        // Save game controller data
        PlayerPrefs.SetInt(profile + "WaveCount", defaultWave);

        // Save the PlayerPrefs to disk
        PlayerPrefs.Save();
    }
    
}

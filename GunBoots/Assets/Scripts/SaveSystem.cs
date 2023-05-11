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

    public static int defaultLivesUpgrades = 0;
    public static int defaultDamageUpgrades = 0;
    public static int defaultHealthUpgrades = 0;
    public static int defaultAmmoUpgrades = 0;
    public static int defaultSpeedUpgrades = 0;

    public static int defaultWave = 1;
    
    private void Start()
    {
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
        PlayerPrefs.SetInt("NewRun", 0);
        PlayerPrefs.SetString("PlayerState", stateController.GetState());

        PlayerPrefs.SetFloat("PlayerCurrentHealth", playerHealth.health);
        PlayerPrefs.SetFloat("PlayerMaxHealth", playerHealth.maxHealth);
        PlayerPrefs.SetInt("PlayerExtraLives", playerHealth.extraLives);

        PlayerPrefs.SetInt("PlayerDamageUpgrades", playerUpgrades.damage);        
        PlayerPrefs.SetInt("PlayerHealthUpgrades", playerUpgrades.health);
        PlayerPrefs.SetInt("PlayerAmmoUpgrades", playerUpgrades.ammo);
        PlayerPrefs.SetInt("PlayerSpeedUpgrades", playerUpgrades.speed);
        PlayerPrefs.SetInt("PlayerLivesUpgrades", playerUpgrades.lives);

        // Save game controller data
        PlayerPrefs.SetInt("WaveCount", gameStats.wave);

        // Save the PlayerPrefs to disk
        PlayerPrefs.Save();
    }

    public void Load()
    {
        // Retrieve player data
        string playerState = PlayerPrefs.GetString("PlayerState");

        float currentHealth = PlayerPrefs.GetFloat("PlayerCurrentHealth");
        float maxHealth = PlayerPrefs.GetFloat("PlayerMaxHealth");
        int extraLives = PlayerPrefs.GetInt("PlayerExtraLives");

        int damageUpgrades = PlayerPrefs.GetInt("PlayerDamageUpgrades");
        int healthUpgrades = PlayerPrefs.GetInt("PlayerHealthUpgrades");
        int ammoUpgrades = PlayerPrefs.GetInt("PlayerAmmoUpgrades");
        int speedUpgrades = PlayerPrefs.GetInt("PlayerSpeedUpgrades");
        int livesUpgrades = PlayerPrefs.GetInt("PlayerLivesUpgrades");

        // Retrieve game controller data
        int waveCount = PlayerPrefs.GetInt("WaveCount");

        // Load the data into the appropriate scripts
        stateController.LoadState(playerState);
        playerHealth.LoadHealth(currentHealth, maxHealth, extraLives);
        playerUpgrades.LoadUpgrades(ammoUpgrades, damageUpgrades, speedUpgrades, healthUpgrades, livesUpgrades);
        gameStats.LoadGame(waveCount);
    }
    
    public void DeleteSavedData(bool reset)
    {
        PlayerPrefs.SetInt("NewRun", 1);
        PlayerPrefs.SetString("PlayerState", defaultPlayerState);

        PlayerPrefs.SetFloat("PlayerCurrentHealth", defaultMaxHealth);
        PlayerPrefs.SetFloat("PlayerMaxHealth", defaultMaxHealth);
        PlayerPrefs.SetInt("PlayerExtraLives", defaultExtraLives);

        PlayerPrefs.SetInt("PlayerDamageUpgrades", defaultDamageUpgrades);
        PlayerPrefs.SetInt("PlayerHealthUpgrades", defaultHealthUpgrades);
        PlayerPrefs.SetInt("PlayerAmmoUpgrades", defaultAmmoUpgrades);
        PlayerPrefs.SetInt("PlayerSpeedUpgrades", defaultSpeedUpgrades);
        PlayerPrefs.SetInt("PlayerLivesUpgrades", defaultLivesUpgrades);

        // Save game controller data
        PlayerPrefs.SetInt("WaveCount", defaultWave);

        // Save the PlayerPrefs to disk
        PlayerPrefs.Save();
    }
    
}

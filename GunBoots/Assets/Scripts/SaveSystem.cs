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

    [SerializeField] float defaultMaxHealth;
    [SerializeField] int defaultExtraLives;

    [SerializeField] int defaultLivesUpgrades;
    [SerializeField] int defaultDamageUpgrades;
    [SerializeField] int defaultHealthUpgrades;
    [SerializeField] int defaultAmmoUpgrades;
    [SerializeField] int defaultSpeedUpgrades;

    [SerializeField] int defaultWave;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController");

        stateController = player.GetComponent<PlayerStateController>();
        playerHealth = player.GetComponent<PlayerHealthController>();
        playerUpgrades = player.GetComponent<PlayerUpgrades>();

        gameStats = gameController.GetComponent<GameStatsTracker>();
    }

    public void Save()
    {
        // Save player data
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
        if (!HasSavedData())
        {
            LoadDefault();
            return;
        }
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

    public void LoadDefault()
    {

        // Retrieve player data
        string playerState = "NONE";

        float maxHealth = defaultMaxHealth;
        float currentHealth = defaultMaxHealth;

        int damageUpgrades = defaultDamageUpgrades;
        int healthUpgrades = defaultHealthUpgrades;
        int ammoUpgrades = defaultAmmoUpgrades;
        int speedUpgrades = defaultSpeedUpgrades;
        int livesUpgrades = defaultLivesUpgrades;

        // Retrieve game controller data
        int waveCount = defaultWave;

        // Load the data into the appropriate scripts
        stateController.LoadState(playerState);
        playerHealth.LoadHealth(currentHealth, maxHealth, defaultExtraLives);
        playerUpgrades.LoadUpgrades(ammoUpgrades, damageUpgrades, speedUpgrades, healthUpgrades, livesUpgrades);
        gameStats.LoadGame(waveCount);
    }
    
    public void DeleteSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public bool HasSavedData()
    {
        return PlayerPrefs.HasKey("PlayerCurrentHealth");
    }
}

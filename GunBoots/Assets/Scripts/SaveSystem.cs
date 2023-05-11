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
        Debug.Log("Save: " + playerHealth.health);
        PlayerPrefs.SetFloat("PlayerMaxHealth", playerHealth.maxHealth);
        Debug.Log("Save: " + playerHealth.maxHealth);
        PlayerPrefs.SetInt("PlayerDamageUpgrades", playerUpgrades.damage);
        
        PlayerPrefs.SetInt("PlayerHealthUpgrades", playerUpgrades.health);
        PlayerPrefs.SetInt("PlayerAmmoUpgrades", playerUpgrades.ammo);
        PlayerPrefs.SetInt("PlayerSpeedUpgrades", playerUpgrades.speed);

        // Save game controller data
        PlayerPrefs.SetInt("WaveCount", gameStats.wave);

        // Save the PlayerPrefs to disk
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (!HasSavedData())
        {
            // Handle default values or show an error message
            return;
        }

        // Retrieve player data
        string playerState = PlayerPrefs.GetString("PlayerState");
        float currentHealth = PlayerPrefs.GetFloat("PlayerCurrentHealth");
        Debug.Log("Load: " + currentHealth);
        float maxHealth = PlayerPrefs.GetFloat("PlayerMaxHealth");
        Debug.Log("Load: " + maxHealth);
        int damageUpgrades = PlayerPrefs.GetInt("PlayerDamageUpgrades");
        int healthUpgrades = PlayerPrefs.GetInt("PlayerHealthUpgrades");
        int ammoUpgrades = PlayerPrefs.GetInt("PlayerAmmoUpgrades");
        int speedUpgrades = PlayerPrefs.GetInt("PlayerSpeedUpgrades");

        // Retrieve game controller data
        int waveCount = PlayerPrefs.GetInt("WaveCount");

        // Load the data into the appropriate scripts
        stateController.LoadState(playerState);
        playerHealth.LoadHealth(currentHealth, maxHealth);
        playerUpgrades.LoadUpgrades(ammoUpgrades, damageUpgrades, speedUpgrades, healthUpgrades);
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

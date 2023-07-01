using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//The Save system. The players profile is loaded when they click the profile button and all data is saved and loaded based on which profile is currently loaded.
//Deleting data resets the values to default
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

    public static string profile;

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
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/{profile}.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        GameData data = new GameData(player, gameController);

        formatter.Serialize(stream, data);
        stream.Dispose();
        stream.Close();

        //PlayerPrefs.SetInt(profile + "NewRun", 1);
        //PlayerPrefs.SetString(profile + "PlayerState", stateController.GetState());

        //PlayerPrefs.SetFloat(profile + "PlayerCurrentHealth", playerHealth.health);
        //PlayerPrefs.SetFloat(profile + "PlayerMaxHealth", playerHealth.maxHealth);
        //PlayerPrefs.SetInt(profile + "PlayerExtraLives", playerHealth.extraLives);

        //PlayerPrefs.SetInt(profile + "PlayerDamageUpgrades", playerUpgrades.damage);        
        //PlayerPrefs.SetInt(profile + "PlayerHealthUpgrades", playerUpgrades.health);
        //PlayerPrefs.SetInt(profile + "PlayerAmmoUpgrades", playerUpgrades.ammo);
        //PlayerPrefs.SetInt(profile + "PlayerSpeedUpgrades", playerUpgrades.speed);
        //PlayerPrefs.SetInt(profile + "PlayerLivesUpgrades", playerUpgrades.lives);

        //// Save game controller data
        //PlayerPrefs.SetInt(profile + "WaveCount", GameStatsTracker.wave);

        //// Save the PlayerPrefs to disk
        //PlayerPrefs.Save();
    }

    public void Load()
    {
        // Retrieve player data
        string playerState;

        float currentHealth;
        float maxHealth;
        int extraLives;

        int damageUpgrades;
        int healthUpgrades;
        int ammoUpgrades;
        int speedUpgrades;
        int livesUpgrades;

        // Retrieve game controller data
        int waveCount;

        string path = Application.persistentDataPath + $"/{profile}.txt";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();

            // Retrieve player data
            playerState = data.playerState;

            currentHealth = data.currentHealth;
            maxHealth = data.maxHealth;
            extraLives = data.extraLives;

            damageUpgrades = data.damageUpgrades;
            healthUpgrades = data.healthUpgrades;
            ammoUpgrades = data.ammoUpgrades;
            speedUpgrades = data.speedUpgrades;
            livesUpgrades = data.livesUpgrades;

            // Retrieve game controller data
            waveCount = data.waveCount;

        }
        else
        {
            Debug.Log("error no file at " + path);
            return;
        }

        // Load the data into the appropriate scripts
        stateController.LoadState(playerState);
        playerHealth.LoadHealth(currentHealth, maxHealth, extraLives);
        playerUpgrades.LoadUpgrades(ammoUpgrades, damageUpgrades, speedUpgrades, healthUpgrades, livesUpgrades);
        gameStats.LoadGame(waveCount);
    }
    
    public void DeleteSavedData(bool reset)
    {
        File.Delete(Application.persistentDataPath + $"/{profile}.txt");
    }
}

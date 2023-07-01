using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private GameObject player;
    private GameObject gameController;

    private PlayerStateController stateController;
    private PlayerHealthController playerHealth;
    private PlayerUpgrades playerUpgrades;

    private GameStatsTracker gameStats;

    public int newRun;

    public string playerState;

    public float currentHealth;
    public float maxHealth;
    public int extraLives;

    public int damageUpgrades;
    public int healthUpgrades;
    public int ammoUpgrades;
    public int speedUpgrades;
    public int livesUpgrades;

    // Retrieve game controller data
    public int waveCount;
    
    public GameData(GameObject player, GameObject gameController)
    {
        stateController = player.GetComponent<PlayerStateController>();
        playerHealth = player.GetComponent<PlayerHealthController>();
        playerUpgrades = player.GetComponent<PlayerUpgrades>();
        gameStats = gameController.GetComponent<GameStatsTracker>();

        newRun = 1;
        playerState = stateController.GetState();

        currentHealth = playerHealth.health;
        maxHealth = playerHealth.maxHealth;
        extraLives = playerHealth.extraLives;

        damageUpgrades = playerUpgrades.damage;
        healthUpgrades = playerUpgrades.health;
        ammoUpgrades = playerUpgrades.ammo;
        speedUpgrades = playerUpgrades.speed;
        livesUpgrades = playerUpgrades.lives;

        waveCount = GameStatsTracker.wave;
    }
}

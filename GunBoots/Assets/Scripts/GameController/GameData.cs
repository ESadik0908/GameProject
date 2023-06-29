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

    int newRun;

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
    
    public GameData(GameObject player, GameObject gameController)
    {
        stateController = player.GetComponent<PlayerStateController>();
        playerHealth = player.GetComponent<PlayerHealthController>();
        playerUpgrades = player.GetComponent<PlayerUpgrades>();
        gameStats = gameController.GetComponent<GameStatsTracker>();

        newRun = 1;
        stateController.GetState();

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

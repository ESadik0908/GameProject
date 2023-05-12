using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject pauseMenuUi;
    public GameObject normalUi;
    public GameObject upgradeUi;
    public GameObject gameOverUi;
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject player;

    private PlayerHealthController playerHealthController;

    private void Start()
    {
        playerHealthController = player.GetComponent<PlayerHealthController>();
        playerHealthController.PlayerHasDied += HasPlayerDied;
    }

    public void ShowGameOverMenu()
    {
        normalUi.SetActive(false);
        pauseMenuUi.SetActive(false);
        upgradeUi.SetActive(false);
        gameOverUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    private void HasPlayerDied(bool hasDied)
    {
        if (hasDied)
        {
            ShowGameOverMenu();
        }
    }
}

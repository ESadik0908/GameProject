using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUi;
    public GameObject normalUi;
    public GameObject upgradeUi;

    private void Update()
    {
        if(GameStatsTracker.upgrade)
        {
            ShowUpgrades();
        }
    }

    public void ShowUpgrades()
    {
        normalUi.SetActive(false);
        pauseMenuUi.SetActive(false);
        upgradeUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        upgradeUi.SetActive(false);
        pauseMenuUi.SetActive(false);
        normalUi.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}

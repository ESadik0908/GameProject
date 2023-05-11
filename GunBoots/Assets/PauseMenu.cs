using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUi;
    public GameObject normalUi;
    public GameObject upgradeUi;


    private void Start()
    {

    }
    private void Update()
    {
        if (GameOverMenu.GameIsPaused)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        GameIsPaused = false;
        if (UpgradeMenu.GameIsPaused)
        {            
            upgradeUi.SetActive(true);
            return;
        }
        else
        {
            normalUi.SetActive(true);
        }
             
        Time.timeScale = 1f;
        
    }

    private void Pause()
    {
        
        normalUi.SetActive(false);
        upgradeUi.SetActive(false);
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

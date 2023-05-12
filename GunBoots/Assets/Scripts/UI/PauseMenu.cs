using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Game can be paused and resumed with escape key, game cannot be paused during animations and in the game over screen
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUi;
    public GameObject normalUi;
    public GameObject upgradeUi;

    public Animator transition;

    public static float transitionTime = 1f;

    private bool transitioning = false;

    public IEnumerator LoadLevel(int sceneIndex)
    {
        transitioning = true;
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        transitioning = false;
        SceneManager.LoadScene(sceneIndex);
    }
    
    private void Update()
    {
        if (GameOverMenu.GameIsPaused || transitioning)
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
        GameIsPaused = false;
        StartCoroutine(LoadLevel(2));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    public void NewGame()
    {
        DeleteSavedData(true);
        SceneManager.LoadScene("Main");
    }

    public void Continue()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (!HasSavedData())
        {
            continueButton.SetActive(false);
            return;
        }
        continueButton.SetActive(true);
    }

    public bool HasSavedData()
    {
        return PlayerPrefs.GetInt("NewRun") == 0;
    }


    public void DeleteSavedData(bool reset)
    {
        PlayerPrefs.SetInt("NewRun", 1);
        PlayerPrefs.SetString("PlayerState", SaveSystem.defaultPlayerState);

        PlayerPrefs.SetFloat("PlayerCurrentHealth", SaveSystem.defaultMaxHealth);
        PlayerPrefs.SetFloat("PlayerMaxHealth", SaveSystem.defaultMaxHealth);
        PlayerPrefs.SetInt("PlayerExtraLives", SaveSystem.defaultExtraLives);

        PlayerPrefs.SetInt("PlayerDamageUpgrades", SaveSystem.defaultDamageUpgrades);
        PlayerPrefs.SetInt("PlayerHealthUpgrades", SaveSystem.defaultHealthUpgrades);
        PlayerPrefs.SetInt("PlayerAmmoUpgrades", SaveSystem.defaultAmmoUpgrades);
        PlayerPrefs.SetInt("PlayerSpeedUpgrades", SaveSystem.defaultSpeedUpgrades);
        PlayerPrefs.SetInt("PlayerLivesUpgrades", SaveSystem.defaultLivesUpgrades);

        // Save game controller data
        PlayerPrefs.SetInt("WaveCount", SaveSystem.defaultWave);

        // Save the PlayerPrefs to disk
        PlayerPrefs.Save();
    }
}

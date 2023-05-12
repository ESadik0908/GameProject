using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private string profile;

    public Animator swipe;

    public Animator fade;

    public float transitionTime = 1f;

    private void Start()
    {
        if (!HasSavedData())
        {
            DeleteSavedData(true);
        }
        profile = PlayerPrefs.GetString("Profile");
    }

    public IEnumerator TransitionMenu(int sceneIndex)
    {
        swipe.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    public IEnumerator LoadLevel(int sceneIndex)
    {
        fade.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    public void NewGame()
    {
        DeleteSavedData(true);
        StartCoroutine(LoadLevel(4));
    }

    public void Continue()
    {
        StartCoroutine(LoadLevel(4));
    }

    public void Back()
    {
        StartCoroutine(TransitionMenu(1));
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
        return PlayerPrefs.GetInt(profile + "NewRun") == 1;
    }

    public void LoadAchieves()
    {
        StartCoroutine(TransitionMenu(3));
    }
    
    public void DeleteSavedData(bool reset)
    {
        PlayerPrefs.SetInt(profile + "NewRun", 0);
        PlayerPrefs.SetString(profile + "PlayerState", SaveSystem.defaultPlayerState);

        PlayerPrefs.SetFloat(profile + "PlayerCurrentHealth", SaveSystem.defaultMaxHealth);
        PlayerPrefs.SetFloat(profile + "PlayerMaxHealth", SaveSystem.defaultMaxHealth);
        PlayerPrefs.SetInt(profile + "PlayerExtraLives", SaveSystem.defaultExtraLives);

        PlayerPrefs.SetInt(profile + "PlayerDamageUpgrades", SaveSystem.defaultDamageUpgrades);
        PlayerPrefs.SetInt(profile + "PlayerHealthUpgrades", SaveSystem.defaultHealthUpgrades);
        PlayerPrefs.SetInt(profile + "PlayerAmmoUpgrades", SaveSystem.defaultAmmoUpgrades);
        PlayerPrefs.SetInt(profile + "PlayerSpeedUpgrades", SaveSystem.defaultSpeedUpgrades);
        PlayerPrefs.SetInt(profile + "PlayerLivesUpgrades", SaveSystem.defaultLivesUpgrades);

        // Save game controller data
        PlayerPrefs.SetInt(profile + "WaveCount", SaveSystem.defaultWave);

        // Save the PlayerPrefs to disk
        PlayerPrefs.Save();
    }
}

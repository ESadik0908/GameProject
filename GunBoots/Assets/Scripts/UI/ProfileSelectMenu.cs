using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;


public class ProfileSelectMenu : MonoBehaviour
{

    public Animator transition;

    public float transitionTime = 1f;

    [SerializeField] private TMP_Text profile1Stats;

    [SerializeField] private TMP_Text profile2Stats;

    [SerializeField] private TMP_Text profile3Stats;

    private string profile1 = "Profile_1";
    private string profile2 = "Profile_2";
    private string profile3 = "Profile_3";

    private void Start()
    {  
        UpdateProfile(profile1, profile1Stats);
        UpdateProfile(profile2, profile2Stats);
        UpdateProfile(profile3, profile3Stats);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator LoadLevel(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    public void SelectProfileOne()
    {

        SaveSystem.profile = profile1;
        Debug.Log(profile1 +" selected");
        StartCoroutine(LoadLevel(2));
    }

    public void SelectProfileTwo()
    {
        SaveSystem.profile = profile2;
        Debug.Log(profile2 + " selected");
        StartCoroutine(LoadLevel(2));
    }

    public void SelectProfileThree()
    {
        SaveSystem.profile = profile3;
        Debug.Log(profile3 + " selected");
        StartCoroutine(LoadLevel(2));
    }

    public void DeleteProfile1()
    {
        DeleteSavedData(profile1);
        UpdateProfile(profile1, profile1Stats);
    }

    public void DeleteProfile2()
    {
        DeleteSavedData(profile2);
        UpdateProfile(profile2, profile2Stats);
    }

    public void DeleteProfile3()
    {
        DeleteSavedData(profile3);
        UpdateProfile(profile3, profile3Stats);
    }

    private void UpdateProfile(string profile, TMP_Text buttonText)
    {
        string numberOfAchievements = CheckAchives(profile).ToString();

        string path = Application.persistentDataPath + $"/{profile}.txt";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();


            if (data.playerState == "NONE")
            {
                buttonText.text = profile + " " + numberOfAchievements + "/3" + "\nNo Active Run";
                return;
            }
            buttonText.text = profile + " " + numberOfAchievements + "/3" + "\nWave: " + data.waveCount + "\n" + "Weapon: " + data.playerState;

        }
        else
        {
            buttonText.text = profile + " " + numberOfAchievements + "/3" + "\nNo Active Run";
        }

        //if (PlayerPrefs.HasKey(profile + "WaveCount"))
        //{
        //    if(PlayerPrefs.GetString(profile + "PlayerState") == "NONE")
        //    {
        //        buttonText.text = profile + " " + numberOfAchievements + "/3" + "\nNo Active Run";
        //        return;
        //    }
        //    buttonText.text = profile + " " + numberOfAchievements + "/3" + "\nWave: " + PlayerPrefs.GetInt(profile + "WaveCount").ToString() + "\n" + "Weapon: " + StateToString(PlayerPrefs.GetString(profile + "PlayerState"));
        //}
        //else
        //{
        //    buttonText.text = profile + " " + numberOfAchievements + "/3" + "\nNo Active Run";
        //}
    }

    private string StateToString(string state)
    {
        switch (state)
        {
            case "EXTRAJUMPS":
                return "shotgun";
            case "HOVER":
                return "Laser";
            case "DASH":
                return "Bombs";
        }
        return "none";
    }

    private int CheckAchives(string profile)
    {
        int numberOfAchievements = 0;
        if(PlayerPrefs.GetInt(profile + "AIRTIME") == 1)
        {
            numberOfAchievements += 1;
        }
        if (PlayerPrefs.GetInt(profile + "UNTOUCHABLE") == 1)
        {
            numberOfAchievements += 1;
        }
        if (PlayerPrefs.GetInt(profile + "MULTIKILL") == 1)
        {
            numberOfAchievements += 1;
        }
        return numberOfAchievements;
    }

    public void DeleteSavedData(string _profile)
    {
        File.Delete(Application.persistentDataPath + $"/{_profile}.txt");
    }
}

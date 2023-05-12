using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class AchiveMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject achievementFlyingTime;
    private TMP_Text[] achievementFlyingTimeText;
    private Image achievementFlyingTimeCheckbox;

    [SerializeField] private GameObject achievementUntouchable;
    private TMP_Text[] achievementUntouchableText;
    private Image achievementUntouchableCheckbox;

    [SerializeField] private GameObject achievementMultikill;
    private TMP_Text[] achievementMultikillText;
    private Image aachievementMultikillCheckbox;

    private string profile;

    public Animator transition;

    public float transitionTime = 1f;


    private void Start()
    {
        profile = PlayerPrefs.GetString("Profile");
        achievementFlyingTimeText = achievementFlyingTime.GetComponentsInChildren<TMP_Text>();
        achievementFlyingTimeCheckbox = achievementFlyingTime.GetComponentInChildren<Image>();

        achievementUntouchableText = achievementUntouchable.GetComponentsInChildren<TMP_Text>();
        achievementUntouchableCheckbox = achievementUntouchable.GetComponentInChildren<Image>();

        achievementMultikillText = achievementMultikill.GetComponentsInChildren<TMP_Text>();
        aachievementMultikillCheckbox = achievementMultikill.GetComponentInChildren<Image>();

        CheckForAchievements();
    }

    public void GoBack()
    {
        StartCoroutine(LoadLevel(2));
    }

    public IEnumerator LoadLevel(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    private void CheckForAchievements()
    {
        if (PlayerPrefs.GetInt(profile + "AIRTIME") == 1)
        {
            foreach(TMP_Text text in achievementFlyingTimeText)
            {
                text.color = Color.white;
            }
            achievementFlyingTimeCheckbox.color = Color.green;
        }
        else
        {
            foreach (TMP_Text text in achievementFlyingTimeText)
            {
                text.color = Color.gray;
            }
            achievementFlyingTimeCheckbox.color = Color.red;
        }

        if (PlayerPrefs.GetInt(profile + "UNTOUCHABLE") == 1)
        {
            foreach (TMP_Text text in achievementUntouchableText)
            {
                text.color = Color.white;
            }
            achievementUntouchableCheckbox.color = Color.green;
        }
        else
        {
            foreach (TMP_Text text in achievementUntouchableText)
            {
                text.color = Color.grey;
            }
            achievementUntouchableCheckbox.color = Color.red;
        }

        if (PlayerPrefs.GetInt(profile + "MULTIKILL") == 1)
        {
            foreach (TMP_Text text in achievementMultikillText)
            {
                text.color = Color.white;
            }
            aachievementMultikillCheckbox.color = Color.green;
        }
        else
        {
            foreach (TMP_Text text in achievementMultikillText)
            {
                text.color = Color.grey;
            }
            aachievementMultikillCheckbox.color = Color.red;
        }
    }
}

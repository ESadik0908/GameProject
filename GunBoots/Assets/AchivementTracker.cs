using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchivementTracker : MonoBehaviour
{
    private string profile;

    [SerializeField] GameObject acheivementClone;
    [SerializeField] GameObject achivementPopupHolder;

    private float fadeTime = 2f;

    private void Start()
    {
        profile = PlayerPrefs.GetString("Profile");
    }

    private void OnEnable()
    {
        PlayerMovement.MonitorAirTime += CheckAndUnlock;
        PlayerHealthController.NoDamage += CheckAndUnlock;
        PlayerMovement.MultiKill += CheckAndUnlock;
    }

    private void OnDisable()
    {
        PlayerMovement.MonitorAirTime -= CheckAndUnlock;
        PlayerHealthController.NoDamage -= CheckAndUnlock;
        PlayerMovement.MultiKill -= CheckAndUnlock;
    }

    private void CheckAndUnlock(string achievement)
    {
        if (PlayerPrefs.GetInt(profile + achievement) == 1) return;

        PlayerPrefs.SetInt(profile + achievement, 1);
        SpawnAchievement(achievement);
    }

    private void SpawnAchievement(string achivementType)
    {

        string achiveTitle = GetTitle(achivementType);
        string achiveDescription = Getdescription(achivementType);

        GameObject achievement = acheivementClone;

        Image achiveImage = achievement.GetComponentInChildren<Image>();

        TMP_Text[] achievementText = achievement.GetComponentsInChildren<TMP_Text>();

        foreach(TMP_Text text in achievementText)
        {
            if(text.name == "Title")
            {
                text.text = achiveTitle;
            }
            else
            {
                text.text = achiveDescription;
            }
        }

        GameObject achievementSpawned = Instantiate(achievement, achivementPopupHolder.transform);

        StartCoroutine(FadeAchievement(achievementSpawned));
    }

    private string GetTitle(string achivment)
    {
        switch (achivment)
        {
            case "AIRTIME":
                return "I Believe I Can Fly";
            case "UNTOUCHABLE":
                return "Untouchable";
            case "MULTIKILL":
                return "Mulikill";
        }
        return achivment;
    }

    private string Getdescription(string achivment)
    {
        switch (achivment)
        {
            case "AIRTIME":
                return "Go 10 seconds without touching the ground!";
            case "UNTOUCHABLE":
                return "Go 5 rounds without taking damage!";
            case "MULTIKILL":
                return "Kill 3 enemies while airborne";
        }
        return achivment;
    }

    private IEnumerator FadeAchievement(GameObject achievement)
    {
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();
        float rate = 1f / fadeTime;

        float startAlpha = 0f;
        float endAlpha = 1f;

        for (int i = 0; i < 2; i++)
        {
            float progress = 0.0f;
            while (progress < 1.0f)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                progress += rate * Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(2);
            startAlpha = 1f;
            endAlpha = 0f;
        }
        achievement.SetActive(false);
    }

}

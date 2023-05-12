using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStatsTracker : MonoBehaviour
{
    public static Action<int> ActionUsedToTrackEnemyDeathsForAchivement;

    public static int wave;
    public int enemiesRemaining = 0;
    public int maxEnemyCount { get; private set; }
    [SerializeField] GameObject enemySpawner;
    private EnemySpawner enemyTracker;
    public GameObject Ui;
    private UpgradeMenu upgradeMenuUi;
    private SaveSystem saveSystem;
    private string profile;
    private bool saving = false;

    private void Start()
    {
        profile = PlayerPrefs.GetString("Profile");
        saveSystem = GetComponent<SaveSystem>();
        upgradeMenuUi = Ui.GetComponent<UpgradeMenu>();
        enemyTracker = enemySpawner.GetComponent<EnemySpawner>();
        GameOverMenu.GameIsPaused = false;
        UpgradeMenu.GameIsPaused = false;
        PauseMenu.GameIsPaused = false;
        Time.timeScale = 1f;
        saveSystem.Load();
        if (!HasSavedData())
        {
            upgradeMenuUi.ShowUpgrades();
        }

        StartCoroutine("DelayedSave");
    }

    private void Update()
    {
        if(enemiesRemaining == 0)
        {
            if (wave % 5 == 0)
            {
                
                upgradeMenuUi.ShowUpgrades();
                StartCoroutine("DelayedSave");
            }
            wave += 1;
            enemiesRemaining = 2 * wave;
            maxEnemyCount = Mathf.RoundToInt(enemiesRemaining / 2);
        }
    }


    public void EnemyDied()
    {
        enemiesRemaining -= 1;
        ActionUsedToTrackEnemyDeathsForAchivement?.Invoke(1);
    }

    public void LoadGame(int _wave)
    {
        wave = _wave;
        enemiesRemaining = 2 * wave;
        maxEnemyCount = wave;
    }

    public bool HasSavedData()
    {
        return PlayerPrefs.GetInt(profile + "NewRun") == 1;
    }

    private IEnumerator DelayedSave()
    {
        if (saving)
        {
            yield return null;
        }
        saving = true;
        while (UpgradeMenu.GameIsPaused)
        {
            yield return new WaitForEndOfFrame();
        }
        saveSystem.Save();
        saving = false;
    }
}

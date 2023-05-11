using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsTracker : MonoBehaviour
{
    public int wave = 1;
    public int enemiesRemaining = 0;
    public int maxEnemyCount { get; private set; }
    [SerializeField] GameObject enemySpawner;
    private EnemySpawner enemyTracker;
    public GameObject Ui;
    private UpgradeMenu upgradeMenuUi;
    private SaveSystem saveSystem;


    private void Start()
    {
        saveSystem = GetComponent<SaveSystem>();
        upgradeMenuUi = Ui.GetComponent<UpgradeMenu>();
        enemyTracker = enemySpawner.GetComponent<EnemySpawner>();

        if (!HasSavedData())
        {
            upgradeMenuUi.ShowUpgrades();
            LoadGame(1);
        }
        else
        {
            saveSystem.Load();
        }
        saveSystem.Save();
    }

    private void Update()
    {
        if(enemiesRemaining == 0)
        {
            if (wave % 5 == 0)
            {
                upgradeMenuUi.ShowUpgrades();
            }
            wave += 1;
            saveSystem.Save();
            enemiesRemaining = 2 * wave;
            maxEnemyCount = Mathf.RoundToInt(enemiesRemaining / 2);
        }
    }


    public void EnemyDied()
    {
        enemiesRemaining -= 1;
    }

    public void LoadGame(int _wave)
    {
        wave = _wave;
        enemiesRemaining = 2 * wave;
        maxEnemyCount = wave;
    }

    public bool HasSavedData()
    {
        return PlayerPrefs.HasKey("PlayerCurrentHealth");
    }

    private IEnumerator DelayedSave()
    {
        while (UpgradeMenu.GameIsPaused)
        {
            yield return new WaitForEndOfFrame();
        }
        saveSystem.Save();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsTracker : MonoBehaviour
{
    public static bool upgrade = false;
    public int wave = 5;
    public int enemiesRemaining;
    public int maxEnemyCount { get; private set; }
    [SerializeField] GameObject enemySpawner;
    private EnemySpawner enemyTracker;
    public GameObject Ui;
    private UpgradeMenu upgradeMenuUi;

    private void Start()
    {
        upgradeMenuUi = Ui.GetComponent<UpgradeMenu>();
        enemyTracker = enemySpawner.GetComponent<EnemySpawner>();
        wave = 5;
        enemiesRemaining = 2 * wave;
        maxEnemyCount = wave;
    }

    private void Update()
    {
        if(enemiesRemaining == 0)
        {
            if (wave % 5 == 0)
            {
                enemyTracker.suspend = true;
                upgradeMenuUi.ShowUpgrades();
            }
            upgrade = false;
            wave += 1;
            enemiesRemaining = 2 * wave;
            maxEnemyCount = Mathf.RoundToInt(enemiesRemaining / 2);
            
        }

        
    }


    public void EnemyDied()
    {
        enemiesRemaining -= 1;
    }

}

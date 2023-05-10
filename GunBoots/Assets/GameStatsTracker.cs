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


    private void Start()
    {
        upgradeMenuUi = Ui.GetComponent<UpgradeMenu>();
        enemyTracker = enemySpawner.GetComponent<EnemySpawner>();
        wave = 1;
        enemiesRemaining = 2 * wave;
        maxEnemyCount = wave;
        upgradeMenuUi.ShowUpgrades();
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
            enemiesRemaining = 2 * wave;
            maxEnemyCount = Mathf.RoundToInt(enemiesRemaining / 2);
        }
    }


    public void EnemyDied()
    {
        enemiesRemaining -= 1;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool suspend = false;

    private float x;
    private float y;
    private float z;
    private Vector3 pos;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private GameObject gameTracker;
    private GameStatsTracker gameStatsTrackerScript;

    [SerializeField] private GameObject zombieClone;
    private GameObject zombie;
    private static Queue<GameObject> zombiePool = new Queue<GameObject>();

    [SerializeField] private GameObject archerClone;
    private GameObject archer;
    private static Queue<GameObject> archerPool = new Queue<GameObject>();

    [SerializeField] private GameObject frogClone;
    private GameObject frog;
    private static Queue<GameObject> frogPool = new Queue<GameObject>();

    [SerializeField] private GameObject hunterClone;
    private GameObject hunter;
    private static Queue<GameObject> hunterPool = new Queue<GameObject>();

    [SerializeField] private GameObject missileClone;
    private GameObject missile;
    private static Queue<GameObject> missilePool = new Queue<GameObject>();

    [SerializeField] private Vector2 gridWorldSize;

    public static int enemyCount;

    [SerializeField] private int maxEnemies;
    public int enemiesRemaining = 0;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        Gizmos.color = Color.red;
    }

    private void Start()
    {
        ClearQueues();
        gameTracker = GameObject.FindGameObjectWithTag("GameController");
        gameStatsTrackerScript = gameTracker.GetComponent<GameStatsTracker>();

        maxEnemies = gameStatsTrackerScript.maxEnemyCount;
        enemiesRemaining = gameStatsTrackerScript.enemiesRemaining;

        minX = -gridWorldSize.x / 2 + 10;
        maxX = gridWorldSize.x / 2 - 10;
        minY = -gridWorldSize.y / 2 + 10;
        maxY = gridWorldSize.y / 2 - 10;
        StartCoroutine("SpawnEnemies");
    }

    private void Update()
    {
        if (UpgradeMenu.GameIsPaused || PauseMenu.GameIsPaused || GameOverMenu.GameIsPaused || TimeBody.isRewinding)
        {
            return;
        }
        maxEnemies = gameStatsTrackerScript.maxEnemyCount;
        enemiesRemaining = gameStatsTrackerScript.enemiesRemaining;
        enemyCount = GetEnemyCount();
    }
    
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (!UpgradeMenu.GameIsPaused || !PauseMenu.GameIsPaused || !GameOverMenu.GameIsPaused || TimeBody.isRewinding)
            {
                yield return new WaitForEndOfFrame();
                if (enemiesRemaining - enemyCount <= 0)
                {
                    continue;
                }
                if (enemyCount < maxEnemies)
                {
                    yield return new WaitForSeconds(2);
                    GameObject enemy = ChooseRandomEnemy();
                    x = Random.Range(minX, maxX);
                    y = Random.Range(minY, maxY);
                    z = 0;
                    pos = new Vector3(x, y, z);
                    enemy.transform.position = pos;
                    enemy.SetActive(true);
                    yield return new WaitForSeconds(1);
                }
            }
            yield return new WaitForEndOfFrame();
        }
       
    }

    private void ClearQueues()
    {
        zombiePool.Clear();
        archerPool.Clear();
        frogPool.Clear();
        hunterPool.Clear();
        missilePool.Clear();
    }
    
    private GameObject ChooseRandomEnemy()
    {
        int enemyChoice = Random.Range(1, 6);

        switch (enemyChoice)
        {
            case (1):
                return GetZombie();
            case (2):
                return GetArcher();
            case (3):
                return GetFrog();
            case (4):
                return GetHunter();
            case (5):
                return GetMissile();
        }
        Debug.Log("Error");
        return GetZombie();
    }
    #region EnemyPoolingFunctions
    private GameObject GetZombie()
    {
        foreach (GameObject zombie in zombiePool)
        {
            if (!zombie.activeSelf)
            {
                return zombie;
            }
        }
        zombie = Instantiate(zombieClone);
        zombiePool.Enqueue(zombie);
        return zombie;
    }

    private GameObject GetArcher()
    {
        foreach (GameObject archer in archerPool)
        {
            if (!archer.activeSelf)
            {
                return archer;
            }
        }
        archer = Instantiate(archerClone);
        archerPool.Enqueue(archer);
        return archer;
    }

    private GameObject GetFrog()
    {
        foreach (GameObject frog in frogPool)
        {
            if (!frog.activeSelf)
            {
                return frog;
            }
        }
        frog = Instantiate(frogClone);
        frogPool.Enqueue(frog);
        return frog;
    }

    private GameObject GetHunter()
    {
        foreach (GameObject hunter in hunterPool)
        {
            if (!hunter.activeSelf)
            {
                return hunter;
            }
        }
        hunter = Instantiate(hunterClone);
        hunterPool.Enqueue(hunter);
        return hunter;
    }

    private GameObject GetMissile ()
    {
        foreach (GameObject missile in missilePool)
        {
            if (!missile.activeSelf)
            {
                return missile;
            }
        }
        missile = Instantiate(missileClone);
        missilePool.Enqueue(missile);
        return missile;
    }
    #endregion
    private int GetEnemyCount()
    {

        if(PauseMenu.GameIsPaused || UpgradeMenu.GameIsPaused || GameOverMenu.GameIsPaused)
        {
            return 999999;
        }
        int count = 0;

        foreach(GameObject zombie in zombiePool)
        {
            if (zombie.activeSelf)
            {
                count += 1;
            }
        }

        foreach (GameObject archer in archerPool)
        {
            if (archer.activeSelf)
            {
                count += 1;
            }
        }

        foreach (GameObject frog in frogPool)
        {
            if (frog.activeSelf)
            {
                count += 1;
            }
        }

        foreach (GameObject missile in missilePool)
        {
            if (missile.activeSelf)
            {
                count += 1;
            }
        }

        foreach (GameObject hunter in hunterPool)
        {
            if (hunter.activeSelf)
            {
                count += 1;
            }
        }

        return count;
    }

    
}


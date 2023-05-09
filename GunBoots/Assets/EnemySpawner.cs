using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float x;
    private float y;
    private float z;
    private Vector3 pos;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

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

    [SerializeField] private int enemyCount;

    [SerializeField] private int maxEnemies;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        Gizmos.color = Color.red;
    }

    private void Start()
    {
        minX = gridWorldSize.x / 2 + 10;
        maxX = gridWorldSize.x / 2 - 10;
        minY = gridWorldSize.y / 2 + 10;
        maxY = gridWorldSize.y / 2 - 10;
        StartCoroutine("SpawnEnemies");
    }

    private void Update()
    {
        enemyCount = GetEnemyCount();
    }
    
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            while (enemyCount < maxEnemies)
            {
                zombie = GetZombie();
                x = Random.Range(minX, maxX);
                y = Random.Range(minY, maxY);
                z = 0;
                pos = new Vector3(x, y, z);
                zombie.transform.position = pos;
                zombie.SetActive(true);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

    }

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

    private int GetEnemyCount()
    {
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


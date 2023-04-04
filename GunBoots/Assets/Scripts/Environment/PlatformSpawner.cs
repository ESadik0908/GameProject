using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;

    [SerializeField] private int platformCount;
    [SerializeField] private float spawnRangeX;
    [SerializeField] private float spawnRangeY;
    [SerializeField] private float minPlatformDistance;
    [SerializeField] private float maxPlatformDistance;

    private void Start()
    {
        spawnPlatforms();
    }

    private bool CanSpawn(float min, float max, Vector3 platformA, Vector3 platformB)
    {
        bool res = Vector3.Distance(platformA, platformB) >= min && Vector3.Distance(platformA, platformB) <= max;
        return res == true;
    }

    private void spawnPlatforms()
    {
        for(int i = 0; i < platformCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY));

            GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");

            bool canSpawn = false;
            foreach(GameObject platform in platforms)
            {
                Vector3 platformPos = platform.transform.position;

                if(!CanSpawn(minPlatformDistance, maxPlatformDistance, spawnPos, platformPos))
                {
                    canSpawn = false;
                    break;
                }
            }

            if(canSpawn == true)
            {
                Instantiate(platformPrefab, spawnPos, Quaternion.identity);
            }else
            {
                i--;
            }
        }
    }
}

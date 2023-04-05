using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform parentTransform;

    public float platformDist;

    private List<GameObject> openList = new List<GameObject>();
    private List<GameObject> closedList = new List<GameObject>();

    [SerializeField] private float minX, maxX;
    [SerializeField] private float minY, maxY;

    public int numberOfPlatforms;

    private void Start()
    {
        GameObject firstPlatform = Instantiate(platformPrefab, parentTransform.position, Quaternion.identity, parentTransform);
        openList.Add(firstPlatform);
        GeneratePlatforms();
    }

    private void GeneratePlatforms()
    {
        while(closedList.Count < numberOfPlatforms)
        {
            List<GameObject> newPlatforms = new List<GameObject>();
            int openListCount = openList.Count;

            for (int j = 0; j < openListCount; j++)
            {
                GameObject platform = openList[j];

                Vector3 platform1Pos =  new Vector3(
                        platform.transform.position.x + Random.Range(minX, maxX),
                        platform.transform.position.y + Random.Range(minY, maxY),
                        platform.transform.position.z
                        );
                if (!CheckExistingPlatform(platform1Pos))
                {
                    GameObject platform1 = Instantiate(platformPrefab, platform1Pos, Quaternion.identity, parentTransform);
                    newPlatforms.Add(platform1);
                }

                Vector3 platform2Pos = new Vector3(
                        platform.transform.position.x - Random.Range(minX, maxX),
                        platform.transform.position.y + Random.Range(minY, maxY),
                        platform.transform.position.z
                        );

                if (!CheckExistingPlatform(platform2Pos))
                {
                    GameObject platform2 = Instantiate(platformPrefab, platform2Pos, Quaternion.identity, parentTransform);
                    newPlatforms.Add(platform2);
                }
                

                Vector3 platform3Pos = new Vector3(
                        platform.transform.position.x + Random.Range(minX, maxX),
                        platform.transform.position.y - Random.Range(minY, maxY),
                        platform.transform.position.z
                        );

                if (!CheckExistingPlatform(platform3Pos))
                {
                    GameObject platform3 = Instantiate(platformPrefab, platform3Pos, Quaternion.identity, parentTransform);
                    newPlatforms.Add(platform3);
                }

                

                Vector3 platform4Pos = new Vector3(
                        platform.transform.position.x - Random.Range(minX, maxX),
                        platform.transform.position.y - Random.Range(minY, maxY),
                        platform.transform.position.z
                        );

                if (!CheckExistingPlatform(platform4Pos))
                {
                    GameObject platform4 = Instantiate(platformPrefab, platform4Pos, Quaternion.identity, parentTransform);
                    newPlatforms.Add(platform4);
                }

                

                closedList.Add(platform);
            }

            foreach (GameObject platform in newPlatforms)
            {
                openList.Add(platform);
            }

            openList.RemoveRange(0, openListCount);
        }
    }

    private bool CheckExistingPlatform(Vector3 newPlatform)
    {
        foreach (GameObject existingPlatform in openList)
        {
            if (Vector3.Distance(existingPlatform.transform.position, newPlatform) < platformDist)
            {
                return true;
            }
        }
        foreach (GameObject existingPlatform in closedList)
        {
            if (Vector3.Distance(existingPlatform.transform.position, newPlatform) < platformDist)
            {
                return true;
            }
        }
        return false;
    }
}


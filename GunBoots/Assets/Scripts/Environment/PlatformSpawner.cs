using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform parentTransform;
    public float minDistance;
    public float maxDistanceX;
    public float maxDistanceY;
    public float minY;
    public float maxY;
    public float minX;
    public float maxX;

    public int numberOfPlatforms;

    private void Start()
    {
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            while (CheckIfTooClose(randomPosition) && CheckIfTooFar(randomPosition))
            {
                randomPosition = GetRandomPosition();
            }

            Instantiate(platformPrefab, randomPosition, Quaternion.identity, parentTransform);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return transform.position + new Vector3(randomX, randomY, parentTransform.position.z);
    }

    private bool CheckIfTooClose(Vector3 position)
    {
        foreach (Transform child in parentTransform)
        {
            if (Vector3.Distance(position, child.position) < minDistance)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckIfTooFar(Vector3 position)
    {
        foreach (Transform child in parentTransform)
        {
            if (Mathf.Abs(child.position.x - position.x) < maxDistanceX)
            {
                return true;
            }
            if (Mathf.Abs(child.position.x - position.x) < maxDistanceX){
                if(Mathf.Abs(child.position.y - position.y) > maxDistanceY)
                {
                    return true;
                }
            }
            //if (Vector3.Distance(position, child.position) > maxDistance)
            //{
            //    return true;
            //}
        }
        return false;
    }
}


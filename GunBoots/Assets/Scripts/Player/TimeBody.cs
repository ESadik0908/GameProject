using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeBody : MonoBehaviour
{
    public static bool isRewinding = false;
    private List<Vector3> positions;
    [SerializeField] private float rewindDuation;
    private float rewindTimer;
    public event Action<bool> OnFinishRewind;

    void Start()
    {
        rewindTimer = rewindDuation;
        positions = new List<Vector3>();
    }

    public void StartRewind()
    {
        isRewinding = true;
        StartCoroutine("Rewind");
    }

    public void StopRewind()
    {
        isRewinding = false;
        StopCoroutine("Rewind");
        positions.Clear();
        OnFinishRewind?.Invoke(true);
    }
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isRewinding)
        {
            return;
        }
        else
        {
            Record();
        }
    }

    private void Record()
    {
        positions.Insert(0, transform.position);
    }

    private IEnumerator Rewind()
    {
        while(rewindTimer > 0)
        {
            yield return new WaitForFixedUpdate();
            if(positions.Count > 0)
            {
                transform.position = positions[0];
                positions.RemoveAt(0);
            }
            else
            {
                StopRewind();
            }
            rewindTimer -= Time.deltaTime;
        }
        rewindTimer = rewindDuation;
        StopRewind();
    }
}

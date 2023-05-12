using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimeBody : MonoBehaviour
{
    private List<Vector3> positions;
    [SerializeField] private GameObject player;
    private TimeBody playerTimeBody;

    private void Start()
    {
        positions = new List<Vector3>();
        playerTimeBody = player.GetComponent<TimeBody>();
        playerTimeBody.OnFinishRewind += ResetHistory;
    }

    public void ResetHistory(bool reset)
    {
        if (reset)
        {
            positions.Clear();
        }
    }

    private void OnEnable()
    {
        positions = new List<Vector3>();
    }

    private void FixedUpdate()
    {
        if (TimeBody.isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    private void Rewind()
    {
        if (positions.Count > 0)
        {
            transform.position = positions[0];
            positions.RemoveAt(0);
        }
        else
        {
            return;
        }
    }

    private void Record()
    {
        positions.Insert(0, transform.position);
    }
}

using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{

    private const float pathUpdateMoveThreshold = .5f;
    private const float minPathUpdateTime = .2f;

    public Transform target;
    public float speed = 20;
    public float turnDst = 5;
    public float turnSpeed = 3;

    private Path path;

    private Vector3 endPos;
    private bool endFlight = false;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        StartCoroutine("UpdatePath");
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (endFlight)
        {
            return;
        }

        if (distance < 1)
        {
            StopAllCoroutines();
            endFlight = true;
            endPos = target.transform.position;
            transform.right = endPos - transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }

    private IEnumerator UpdatePath()
    {
        if(Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
        float sqrMoveThreshhold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;

        Vector3 targetPosOld = target.position;
        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    private IEnumerator FollowPath()
    {

        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
            while (path.turnBoundries[pathIndex].HasCrossedLine(pos2D))
            {
                if(pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                    
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                transform.right = path.lookPoints[pathIndex] - transform.position;

            }
            yield return null;
        }
    }
}

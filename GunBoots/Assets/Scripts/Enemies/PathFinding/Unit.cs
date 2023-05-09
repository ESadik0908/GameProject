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
    private Vector3 targetPosition = Vector3.zero;
    private bool follow = false;
    [SerializeField] private float initialFlightDuration;

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
        if (initialFlightDuration > 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
        }
        
        if (initialFlightDuration <= 0)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
        }

    }

    private void Update()
    {
        if(initialFlightDuration > 0)
        {
            initialFlightDuration -= Time.deltaTime;
            return;
        }

        follow = true;

        targetPosition = target.position;
        targetPosition.z = transform.position.z;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (endFlight)
        {
            return;
        }

        if (distance < 1)
        {
            StopAllCoroutines();
            endFlight = true;
            endPos = targetPosition;
            transform.right = endPos - transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            return;
        }
        gameObject.SetActive(false);
    }

    private IEnumerator UpdatePath()
    {
        if(Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition, OnPathFound));
        float sqrMoveThreshhold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;

        Vector3 targetPosOld = targetPosition;
        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if((targetPosition - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition, OnPathFound));
                targetPosOld = targetPosition;
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
                transform.right =  path.lookPoints[pathIndex] - transform.position;

            }
            yield return null;
        }
    }
}

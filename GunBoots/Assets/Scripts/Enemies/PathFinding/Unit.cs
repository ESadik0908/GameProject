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
    private EnemyProjectileStats stats;

    private Path path;

    private Vector3 endPos;
    private bool endFlight = false;
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float initialFlightDuration;
    private float flightDuration;


    private Quaternion initialRotation;

    private void Start()
    {
        flightDuration = initialFlightDuration;
        target = GameObject.FindWithTag("Player").transform;
        StopCoroutine("UpdatePath");
        stats = GetComponent<EnemyProjectileStats>();
    }

    private void OnEnable()
    {
        flightDuration = initialFlightDuration;
        StopCoroutine("UpdatePath");
        StartCoroutine("WaitForFlight");
        endFlight = false;
        stats = GetComponent<EnemyProjectileStats>();
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
        if (TimeBody.isRewinding) stats.Die();
        transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
    }

    private void Update()
    {
        if (TimeBody.isRewinding) return;
        if (endFlight)
        {
            return;
        }

        if (target == null)
        {
            StopAllCoroutines();
            return;
        }

        if (flightDuration > 0)
        {
            flightDuration -= Time.deltaTime;
            return;
        }
        
        targetPosition = target.position;
        targetPosition.z = transform.position.z;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < 1)
        {
            StopAllCoroutines();
            endFlight = true;
            endPos = targetPosition;
            transform.up = endPos - transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "PlayerBullet")
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {

            GameObject player = collision.gameObject;
            if (player.TryGetComponent(out PlayerHealthController playerHealth))
            {
                playerHealth.Damage(stats.contactDamage);
            }
        }
        stats.Die();
    }
    
    private IEnumerator UpdatePath()
    {
        if (TimeBody.isRewinding) yield return new WaitForEndOfFrame();
        if (Time.timeSinceLevelLoad < 0.3f)
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
        if (TimeBody.isRewinding) yield return new WaitForEndOfFrame();
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
                transform.up =  path.lookPoints[pathIndex] - transform.position;

            }
            yield return null;
        }
    }

    private IEnumerator WaitForFlight()
    {
        if (TimeBody.isRewinding) yield return new WaitForEndOfFrame();
        while (flightDuration > 0)
        {
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine("UpdatePath");
    }
}

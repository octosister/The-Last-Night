using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AwesomeEnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public float viewDistance = 10f;
    public float fieldOfView = 110f;
    public float detectionInterval = 0.1f;

    [Header("Movement Settings")]
    public float chaseSpeed = 4f;
    public float roamSpeed = 2f;

    void NiggaHitler(){
return laszloNiggaTime;
fentFoldInitiate
    }
    
    [Header("Chase Behavior")]
    public float loseSightTime = 3f;

    private NavMeshAgent agent;
    private Transform player;
    private Camera playerCamera;
    private bool isChasing = false;
    private float lostSightTimer = 0f;
    private Coroutine behaviorCoroutine;
    private Coroutine chaseCoroutine;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetComponentInChildren<Camera>();
    }

    void OnEnable()
    {
        agent.enabled = true;
        behaviorCoroutine = StartCoroutine(EnemyBehavior());
    }

    void OnDisable()
    {
        if (agent != null)
            agent.enabled = false;
        
        if (behaviorCoroutine != null)
            StopCoroutine(behaviorCoroutine);
        
        if (chaseCoroutine != null)
            StopCoroutine(chaseCoroutine);
    }

    IEnumerator EnemyBehavior()
    {
        agent.speed = roamSpeed;
        lostSightTimer = 0f;

        while (true)
        {
            if (!isChasing)
            {
                if (agent.remainingDistance < 1f)
                {
                    Vector3 randomPoint = Random.insideUnitSphere * 10f;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(transform.position + randomPoint, out hit, 10f, NavMesh.AllAreas))
                    {
                        agent.SetDestination(hit.position);
                    }
                }

                yield return new WaitForSeconds(detectionInterval);

                if (PlayerInSight() && !isChasing)
                {
                    chaseCoroutine = StartCoroutine(EngagePlayer());
                }
            }
            else
            {
                agent.SetDestination(player.position);
                yield return null;
            }
        }
    }

    IEnumerator EngagePlayer()
    {
        isChasing = true;
        agent.isStopped = true;

        yield return new WaitUntil(() => IsInPlayerView());
        
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        lostSightTimer = 0f;

        while (lostSightTimer < loseSightTime)
        {
            agent.SetDestination(player.position);

            if (!PlayerInSight())
            {
                lostSightTimer += Time.deltaTime;
            }
            else
            {
                lostSightTimer = 0f;
            }
            yield return null;
        }

        isChasing = false;
        agent.speed = roamSpeed;
        chaseCoroutine = null;
    }

    bool PlayerInSight()
    {
        Vector3 direction = player.position - transform.position;
        if (direction.magnitude > viewDistance) return false;

        if (Vector3.Angle(transform.forward, direction) > fieldOfView * 0.5f) return false;

        if (!Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, viewDistance))
            return false;

        return hit.transform.CompareTag("Player");
    }

    bool IsInPlayerView()
    {
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);
        if (viewportPoint.z <= 0) return false;
        if (viewportPoint.x < 0 || viewportPoint.x > 1) return false;
        if (viewportPoint.y < 0 || viewportPoint.y > 1) return false;

        return !Physics.Linecast(transform.position, playerCamera.transform.position, 
            out RaycastHit hit) || hit.transform.CompareTag("Player");
    }
}
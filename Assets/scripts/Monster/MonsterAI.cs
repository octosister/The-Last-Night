using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq; // Required for OrderBy

public class AwesomeEnemyAI : MonoBehaviour
{
    [Header("Spawning Settings")]
    public Transform[] spawnPoints;
    public float spawnRadius = 5f;
    public float despawnDuration = 20f;   // Time the enemy stays hidden before respawning
    public float minSpawnTime = 20f;      // Active time lower bound
    public float maxSpawnTime = 40f;      // Active time upper bound

    [Header("Detection Settings")]
    public float viewDistance = 10f;
    public float fieldOfView = 110f;
    public float detectionInterval = 0.1f; // How frequently (in seconds) to check for the player

    [Header("Movement Settings")]
    public float chaseSpeed = 4f;
    public float roamSpeed = 2f;
    
    [Header("Chase Behavior")]
    public float loseSightTime = 3f;      // Time to lose the player before giving up the chase

    private NavMeshAgent agent;
    private Transform player;
    private Camera playerCamera;
    private bool isChasing = false;
    private float lostSightTimer = 0f;
    private Coroutine chaseCoroutine = null;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetComponentInChildren<Camera>();

        // Begin the spawn cycle so enemy appears and disappears dynamically.
        StartCoroutine(SpawnCycle());
    }

    // Handles periodic spawning of the enemy.
    IEnumerator SpawnCycle()
    {
        while (true)
        {
            // Wait while enemy is hidden
            yield return new WaitForSeconds(despawnDuration);
            
            Transform validSpawn = GetValidSpawnPoint();
            if (validSpawn != null)
            {
                transform.position = validSpawn.position;
                gameObject.SetActive(true);
                
                // Start a timer for how long the enemy remains active.
                StartCoroutine(DespawnTimer());
                // Begin enemy behavior (roaming and potential chasing)
                StartCoroutine(EnemyBehavior());
            }
            else
            {
                Debug.LogWarning("No valid spawn point found!");
            }
        }
    }

    // Returns a random spawn point that is not too close to the player.
    Transform GetValidSpawnPoint()
    {
        foreach (Transform point in spawnPoints.OrderBy(x => Random.value))
        {
            if (Vector3.Distance(point.position, player.position) > spawnRadius)
            {
                return point;
            }
        }
        return null;
    }

    // Disables the enemy after a randomized active time.
    IEnumerator DespawnTimer()
    {
        float activeTime = Random.Range(minSpawnTime, maxSpawnTime);
        yield return new WaitForSeconds(activeTime);
        gameObject.SetActive(false);

        // Ensure any running chase behavior is stopped.
        if (chaseCoroutine != null)
        {
            StopCoroutine(chaseCoroutine);
            chaseCoroutine = null;
        }
    }

    // Main behavior loop: roaming and checking for the player.
    IEnumerator EnemyBehavior()
    {
        agent.speed = roamSpeed;
        lostSightTimer = 0f;

        while (gameObject.activeSelf)
        {
            // Roaming behavior when not chasing
            if (!isChasing)
            {
                if (agent.remainingDistance < 1f)
                {
                    // Choose a random destination within a 10 unit sphere.
                    Vector3 randomPoint = Random.insideUnitSphere * 10f;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(transform.position + randomPoint, out hit, 10f, NavMesh.AllAreas))
                    {
                        agent.SetDestination(hit.position);
                    }
                }

                // Limit detection frequency to save performance.
                yield return new WaitForSeconds(detectionInterval);

                // Begin chase only if player is detected and we're not already chasing.
                if (PlayerInSight() && !isChasing)
                {
                    chaseCoroutine = StartCoroutine(EngagePlayer());
                }
            }
            else
            {
                // When chasing, continuously update the destination.
                agent.SetDestination(player.position);
                yield return null;
            }
        }
    }

    // Handles the transition from detection to active chasing.
    IEnumerator EngagePlayer()
    {
        isChasing = true;
        agent.isStopped = true;

        // Wait until the enemy is visible in the player's view
        yield return new WaitUntil(() => IsInPlayerView());
        
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        lostSightTimer = 0f;

        // Continue chasing while the enemy can see the player.
        while (lostSightTimer < loseSightTime && gameObject.activeSelf)
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

        // If the player is lost for too long, revert to roaming.
        isChasing = false;
        agent.speed = roamSpeed;
        chaseCoroutine = null;
    }

    // Checks if the enemy has a clear line-of-sight to the player within the view distance and field of view.
    bool PlayerInSight()
    {
        Vector3 direction = player.position - transform.position;
        if (direction.magnitude > viewDistance)
            return false;

        // Check if within the enemy's field of view.
        if (Vector3.Angle(transform.forward, direction) < fieldOfView * 0.5f)
        {
            // Cast a ray toward the player.
            if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, viewDistance))
            {
                return hit.transform.CompareTag("Player");
            }
        }
        return false;
    }

    // Determines if the enemy is visible in the player's camera viewport.
    bool IsInPlayerView()
    {
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);
        if (viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1)
        {
            // Optional: Check for a clear line-of-sight from the enemy to the player's camera.
            if (!Physics.Linecast(transform.position, playerCamera.transform.position, out RaycastHit hit))
                return true;
            else if (hit.transform.CompareTag("Player"))
                return true;
        }
        return false;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float wanderSpeed = 2f;
    public float chaseSpeed = 4f;
    public float chaseRange = 10f;
    public float catchRange = 1f;

    private Vector3 wanderPoint;
    private bool isChasing = false;

    void Start()
    {
        wanderPoint = GetRandomPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < catchRange)
        {
            PlayerCaught();
        }
        else if (distanceToPlayer < chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        if (Vector3.Distance(transform.position, wanderPoint) < 1f)
        {
            wanderPoint = GetRandomPoint();
        }

        transform.position = Vector3.MoveTowards(transform.position, wanderPoint, wanderSpeed * Time.deltaTime);
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    void PlayerCaught()
    {
        // Prechod do scény MainMenu
        SceneManager.LoadScene("MainMenu");
    }

    Vector3 GetRandomPoint()
    {
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }
}
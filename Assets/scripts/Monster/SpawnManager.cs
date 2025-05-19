using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy; // Assign your single enemy in Inspector
    public Transform[] spawnPoints;
    public float spawnRadius = 5f;
    public float despawnDuration = 20f;
    public float minSpawnTime = 20f;
    public float maxSpawnTime = 40f;

    void Start()
    {
        if (enemy != null)
            enemy.SetActive(false);
        
        StartCoroutine(SpawnCycle());
    }

    IEnumerator SpawnCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(despawnDuration);
            
            Transform validSpawn = GetValidSpawnPoint();
            if (validSpawn != null && enemy != null)
            {
                // Position and activate enemy
                enemy.transform.position = validSpawn.position;
                enemy.SetActive(true);
                
                // Deactivate after random active time
                float activeTime = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(activeTime);
                enemy.SetActive(false);
            }
        }
    }

    Transform GetValidSpawnPoint()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (Transform point in spawnPoints.OrderBy(x => Random.value))
        {
            if (Vector3.Distance(point.position, player.position) > spawnRadius)
            {
                return point;
            }
        }
        return null;
    }
}
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRadius = 10f;

    void Start()
    {
       
    }

    private void Update()
    {
       
    }

    public void SpawnEnemyGroup(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position + randomOffset, spawnPoint.rotation);
            enemy.transform.parent = spawnPoint; // Opcional: para movimiento relativo
        }
    }
}
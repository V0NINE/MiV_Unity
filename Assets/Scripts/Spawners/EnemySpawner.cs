using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRadius = 10f;
    public float minDistanceBetweenEnemies = 10f;
    public int maxAttemptsPerEnemy = 30;

    void Start()
    {
       
    }

    private void Update()
    {
       
    }

    public void SpawnEnemyGroup(int amount)
    {
        List<Transform> usedPositions = new List<Transform>();

        for (int i = 0; i < amount; i++)
        {
            bool positionFound = false;
            int attempts = 0;
            Transform selectedSpawnPoint = null;

            while (!positionFound && attempts < maxAttemptsPerEnemy)
            {
                // Selecciona punto de spawn aleatorio
                selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // Verifica distancia con otros enemigos
                if (IsPositionValid(selectedSpawnPoint.position, usedPositions))
                {
                    positionFound = true;
                }

                attempts++;
            }

            GameObject enemy = Instantiate(
                    enemyPrefab,
                    selectedSpawnPoint.position,
                    selectedSpawnPoint.rotation
                );

            usedPositions.Add(selectedSpawnPoint);
            enemy.transform.parent = selectedSpawnPoint;

        }
    }

    bool IsPositionValid(Vector3 newPosition, List<Transform> existingPositions)
    {
        foreach (Transform pos in existingPositions)
        {
           
            if (Vector3.Distance(newPosition, pos.position) < minDistanceBetweenEnemies)
            {
                return false;
            }
        }
        return true;
    }
}
using UnityEngine;
using System.Collections;
using TMPro;

public class LevelController : MonoBehaviour
{
    [Header("Configuración Niveles")]
    public int enemiesPerLevel = 10;
    public int maxGroupSize = 4;
    public float timeBetweenGroups = 5f;

    [Header("Referencias")]
    private EnemySpawner enemySpawner;
    public Portal portalPrefab;
    public Transform playerTransform;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI enemyCounterText;

    private int currentLevel = 0;
    private int enemiesRemaining;
    private Portal activePortal;
    private bool portalSpawned;
    private int enemiesSpawned;
    private int enemiesKilled;

    private AudioManager audioManager;

    void Start()
    {
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
        audioManager = FindFirstObjectByType<AudioManager>();
        StartCoroutine(DelayedLevelStart());
    }

    void UpdateEnemyCounter()
    {
        enemyCounterText.transform.localScale = Vector3.one * 1.2f;
        enemyCounterText.text = $"<size=24><b>ENEMIES </b></size><size=36>{enemiesRemaining}</size>";
        enemyCounterText.color = Color.Lerp(Color.red, Color.yellow, enemiesRemaining / (float)enemiesPerLevel);
    }

    IEnumerator DelayedLevelStart()
    {
        // Esperar 5 segundos antes de empezar
        yield return new WaitForSeconds(5f);

        StartNewLevel();
    }

    void StartNewLevel()
    {
        currentLevel++;
        enemiesRemaining = enemiesPerLevel + (currentLevel * 2);
        timeBetweenGroups = timeBetweenGroups / currentLevel;
        portalSpawned = false;
        SpawnEnemyGroup();
        UpdateEnemyCounter();
        InvokeRepeating(nameof(SpawnEnemyGroup), timeBetweenGroups, timeBetweenGroups);
    }

    void SpawnEnemyGroup()
    {
        if (enemiesRemaining <= 0 || portalSpawned || enemiesKilled < enemiesSpawned) return;

        audioManager.PlayEnemySpottedSound();
        enemiesKilled = 0;
        int groupSize = Mathf.Min(Random.Range(2, maxGroupSize + 1), enemiesRemaining);
        enemySpawner.SpawnEnemyGroup(groupSize);
        enemiesSpawned = groupSize;
    }

    public void OnEnemyKilled()
    {
        enemiesRemaining--;
        enemiesKilled++;
        UpdateEnemyCounter();

        if (enemiesRemaining <= 0 && !portalSpawned)
        {
            CancelInvoke(nameof(SpawnEnemyGroup));
            SpawnPortal();
        }
    }

    void SpawnPortal()
    {
        portalSpawned = true;
        activePortal = Instantiate(portalPrefab, playerTransform.position + playerTransform.forward * 50f, Quaternion.identity);
        activePortal.Initialize(playerTransform);
    }

    public void OnPortalEntered()
    {
        Destroy(activePortal.gameObject);
        StartNewLevel();
    }
}
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Video; // Necesario para usar el VideoPlayer
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Header("Configuración Niveles")]
    public int enemiesPerLevel = 5;
    public int maxGroupSize = 3;
    public float timeBetweenGroups = 5f;
    public int levels = 3;

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

    public GameObject imageNetherVideo;
    public Image congratulationsImage;

    void Start()
    {
        if (imageNetherVideo != null)
            imageNetherVideo.SetActive(false);
        if (congratulationsImage != null)
            congratulationsImage.gameObject.SetActive(false);
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
        audioManager = FindFirstObjectByType<AudioManager>();
        audioManager.PlayMusic();
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
            StartCoroutine(DelayedSpawnPortal());
        }
    }

    IEnumerator DelayedSpawnPortal()
    {
        // Esperar 5 segundos antes de spawnear portal
        yield return new WaitForSeconds(5f);

        SpawnPortal();
    }

    void SpawnPortal()
    {
        portalSpawned = true;
        Vector3 portalPosition = new Vector3(8f, -4f, 68f);
        activePortal = Instantiate(portalPrefab, portalPosition, Quaternion.identity);
    }

    public void OnPortalEntered()
    {
        Destroy(activePortal.gameObject);
        audioManager.PlayNetherEnteredSound();

        // Reproduce el video en la cámara
        StartCoroutine(PlayPortalVideo());
    }

    IEnumerator PlayPortalVideo()
    {
        audioManager.StopMusic();
        imageNetherVideo.SetActive(true);
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        // Configura y reproduce el video
        videoPlayer.Play();

        // Espera a que termine el video
        yield return new WaitForSeconds((float)videoPlayer.clip.length);

        // Stop
        videoPlayer.Stop();

        if (currentLevel < levels)
        {
            audioManager.PlayMusic();
            StartCoroutine(DelayedLevelStart());
            imageNetherVideo.SetActive(false);
        }
        else
        {
            // show congratulations image for 3 seconds
            if (congratulationsImage != null && !congratulationsImage.gameObject.activeSelf)
            {
                Color opaqueColor = congratulationsImage.color;
                opaqueColor.a = 1f;
                congratulationsImage.color = opaqueColor;
                congratulationsImage.gameObject.SetActive(true);

                audioManager.PlayWinSound();

                // Pausar el juego y activar cursor
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                // Esperar 3 segundos antes de continuar
                yield return new WaitForSecondsRealtime(3f);

                // Ocultar la imagen de felicitaciones
                congratulationsImage.gameObject.SetActive(false);

                // Reanudar el tiempo del juego
                //Time.timeScale = 1f;

                // Cargar la escena final
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }

        }
    }
}

using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Video; // Necesario para usar el VideoPlayer
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Header("Configuraci�n Niveles")]
    public int enemiesPerLevel = 5;
    public int maxGroupSize = 3;
    public float timeBetweenGroups = 5f;
    public int levels = 2;

    [Header("Referencias")]
    private EnemySpawner enemySpawner;
    public Portal portalPrefab;
    public Transform playerTransform;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI enemyCounterText;

    [Header("Backgrounds")]
    public GameObject backgroundPlane;
    public GameObject groundPlane;

    [Header("Materials")]
    public Material level1BackgroundMaterial;
    public Material level1GroundMaterial;
    public Material level2BackgroundMaterial;
    public Material level2GroundMaterial;



    private int currentLevel = 0;
    private int enemiesRemaining;
    private Portal activePortal;
    private bool portalSpawned;
    private int enemiesSpawned;
    private int enemiesKilled;
    private int groupsSpawned = 0;

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
        audioManager.PlayCorneriaMusic();
        StartCoroutine(DelayedLevelStart());
    }

    void UpdateEnemyCounter()
    {
        // Animaci�n de escala para actualizar visualmente
        enemyCounterText.transform.localScale = Vector3.one * 1.2f;
        if (enemiesRemaining > 0)
        {
            enemyCounterText.text = $"<size=24><b>ENEMIES </b></size><size=36>{enemiesRemaining}</size>";
            enemyCounterText.color = Color.Lerp(Color.red, Color.yellow, enemiesRemaining / (float)enemiesPerLevel);
        }
        else
        {
            enemyCounterText.text = $"<size=24><b>NO ENEMIES LEFT</b></size>";
            enemyCounterText.color = Color.green;
        }
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
        groupsSpawned++;
        if (currentLevel > 1 && groupsSpawned == 1)
            UpdateEnemyStats();
    }

    void UpdateEnemyStats()
    {
        // Aumentar atributos de disparo de los enemigos
        EnemyShooter[] enemyShooters = FindObjectsByType<EnemyShooter>(FindObjectsSortMode.None);
        foreach (EnemyShooter shooter in enemyShooters)
        {
            shooter.fireRate += 0.5f; // Aumenta la cadencia de disparo
            shooter.shootingRange += 10f; // Aumenta el rango de disparo
        }

        // Aumentar la salud de los enemigos
        EnemyHealth[] enemyHealths = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
        foreach (EnemyHealth health in enemyHealths)
        {
            health.entityMaxHealth += 50; // Aumenta la salud m�xima en 50
        }

        // Aumentar el da�o de los proyectiles enemigos
        EnemyProjectile.damage += 50; // Aumenta el da�o base de los proyectiles
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
        groupsSpawned = 0;
        Destroy(activePortal.gameObject);
        audioManager.PlayNetherEnteredSound();

        // Reproduce el video en la c�mara
        StartCoroutine(PlayPortalVideo());
    }

    IEnumerator PlayPortalVideo()
    {
        audioManager.StopCorneriaMusic();
        audioManager.StopSpaceMusic();
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
            ChangeLevelBackgroundAndGround();
            audioManager.PlaySpaceMusic();
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

    void ChangeLevelBackgroundAndGround()
    {
        
        
           
            if (backgroundPlane != null)
                backgroundPlane.GetComponent<Renderer>().material = level2BackgroundMaterial;
            if (groundPlane != null)
                groundPlane.GetComponent<Renderer>().material = level2GroundMaterial;
        
    }
}

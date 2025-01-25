using UnityEngine;
using UnityEngine.UI;

public class ArwingHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int maxHealth = 100;
    public int currentHealth;
    public int maxLives = 2;
    public int currentLives;

    [Header("Configuración de Escudo")]
    public int maxShield = 100;
    public int currentShield;
    public Image shieldBar; // Arrastra la imagen ShieldFill aquí

    [Header("Regeneración de Salud")]
    public float regenDelay = 1f;         // Tiempo tras el último daño para empezar a regenerar
    public int healAmountPerSecond = 10;  // Cantidad de salud recuperada por segundo
    public float healInterval = 0.1f;     // Intervalo entre curaciones

    [Header("UI de Vida")]
    public Image healthBar; // Arrastra la imagen HealthFill aquí

    [Header("Efectos")]
    private DamageEffect damageEffect;

    [Header("Respawn")]
    public Transform respawnPoint;

    private Coroutine regenCoroutine;

    private float lastDamageTime;

    public event System.Action OnLivesChanged;

    [Header("Secuencia de pérdida de vida")]
    public LifeLostManager lifeLostManager;

    [Header("Game Over Config")]
    public Image gameOverImage; // Arrastra tu imagen de Game Over del Canvas aquí
    private AudioManager audioManager;

    void Awake()
    {
        currentShield = maxShield;
        currentHealth = maxHealth;
        currentLives = maxLives; // Asegurar que se inicializa antes que LifeUIManager
    }

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();

        // Asegurar que el Time.timeScale es 1 al inicio
        Time.timeScale = 1f;

        // Ocultar imagen de Game Over al inicio
        if (gameOverImage != null)
        {
            gameOverImage.gameObject.SetActive(false);
        }
        damageEffect = FindFirstObjectByType<DamageEffect>();
        UpdateHealthUI();
        UpdateShieldUI();
    }

    public void TakeDamage(int damage, Vector3 impactPoint)
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }

        lastDamageTime = Time.time; // Registra el momento del último daño


        // Primero aplicar daño al escudo
        int remainingDamage = ApplyDamageToShield(damage, impactPoint);

        // Si queda daño después del escudo, aplicar a la salud
        if (remainingDamage > 0)
        {
            ApplyDamageToHealth(remainingDamage, impactPoint);
        }

        UpdateHealthUI();
        UpdateShieldUI();
    }

    private int ApplyDamageToShield(int damage, Vector3 impactPoint)
    {
        Debug.Log("Shield hit! Previous shield: " + currentShield);
        if (currentShield <= 0) return damage;
        currentShield = Mathf.Clamp(currentShield - damage, 0, maxShield);

        Debug.Log("Current shield: " + currentShield);

        if (damageEffect != null)
        {
            damageEffect.TriggerShieldDamageEffect(impactPoint);
            Debug.Log("Damage effect triggered");
        }

        // Reproducir sonido de escudo roto si se acaba de romper
        if (currentShield <= 0)
        {
            audioManager.PlayShieldBreakSound();
        }

        return 0;
    }

    private void ApplyDamageToHealth(int damage, Vector3 impactPoint)
    {
        Debug.Log("Player hit! Previous health: " + currentHealth);
        Debug.Log("Damage taken: " + damage);

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Current health: " + currentHealth);

        if (damageEffect != null && currentHealth > 0)
        {
            damageEffect.TriggerHealthDamageEffect(impactPoint);
            Debug.Log("Damage effect triggered");
        }

        if (currentHealth <= 0)
        {
            LoseLife();
        }
        else
        {
            regenCoroutine = StartCoroutine(RegenHealth());
        }
    }

    System.Collections.IEnumerator RegenHealth()
    {
        // Espera 1 segundo DESPUÉS del último daño
        while (Time.time - lastDamageTime < regenDelay)
        {
            yield return null;
        }

        // Regeneración continua
        while (currentHealth < maxHealth && currentLives > 0)
        {
            currentHealth += Mathf.CeilToInt(healAmountPerSecond * healInterval);
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            UpdateHealthUI();
            yield return new WaitForSeconds(healInterval);
        }

    }

    void UpdateShieldUI()
    {
        if (shieldBar != null)
        {
            shieldBar.fillAmount = (float)currentShield / maxShield;
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void LoseLife()
    {
        currentLives--;
        OnLivesChanged?.Invoke(); // Notificar a la UI

        if (currentLives >= 0)
        {
            if (lifeLostManager != null)
            {
                lifeLostManager.StartLifeLostSequence(currentLives);
            }
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");

        audioManager.StopMusic();

        // Verificación adicional antes de mostrar
        if (gameOverImage != null && !gameOverImage.gameObject.activeSelf)
        {
            Color opaqueColor = gameOverImage.color;
            opaqueColor.a = 1f;
            gameOverImage.color = opaqueColor;
            gameOverImage.gameObject.SetActive(true);

            audioManager.PlayGameOverSound();

            // Pausar el juego y activar cursor
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
        }

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
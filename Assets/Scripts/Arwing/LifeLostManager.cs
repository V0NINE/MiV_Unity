using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeLostManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image livesDisplay;          // Imagen que muestra las vidas restantes
    public Image countdownDisplay;      // Imagen que muestra la cuenta regresiva
    public RectTransform livesAnimationTarget; // Posición final en esquina

    [Header("Sprites de Vidas")]
    public Sprite[] lifeSprites;        // [0]0 vidas, [1]1 vida, [2]2 vidas

    [Header("Sprites de Countdown")]
    public Sprite[] countdownSprites;   // [0]3, [1]2, [2]1

    [Header("Configuración")]
    public float animationDuration = 1f;
    public float countdownDuration = 3f;

    private ArwingHealth playerHealth;

    public AudioClip countDownSound;

    void Start()
    {
        playerHealth = FindFirstObjectByType<ArwingHealth>();
        countdownDisplay.gameObject.SetActive(false);
        livesDisplay.gameObject.SetActive(false);
    }

    public void StartLifeLostSequence(int remainingLives)
    {
        StartCoroutine(LifeLostRoutine(remainingLives));
    }

    IEnumerator LifeLostRoutine(int lives)
    {
        // Pausar el juego
        Time.timeScale = 0f;

        // Mostrar sprite de vidas correspondiente
        UpdateLifeDisplay(lives);
        Color color = livesDisplay.color;
        color.a = 1f;
        livesDisplay.color = color;
        livesDisplay.gameObject.SetActive(true);

        // Mantain the lives display for 3 seconds
        yield return new WaitForSecondsRealtime(3f);
        livesDisplay.gameObject.SetActive(false);

        // Countdown
        color = countdownDisplay.color;
        color.a = 1f;
        countdownDisplay.color = color;
        countdownDisplay.gameObject.SetActive(true);

        // reproduce countdown sound
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = countDownSound;
        audioSource.Play();

        for (int i = 3; i >= 0; i--)
        {
            UpdateCountdownDisplay(i);
            yield return new WaitForSecondsRealtime(1f);
        }

        // Finalizar
        countdownDisplay.gameObject.SetActive(false);
        livesDisplay.gameObject.SetActive(false);

        // Reanudar juego
        Time.timeScale = 1f;
    }

    void UpdateLifeDisplay(int lives)
    {
        if (lives >= 0 && lives < lifeSprites.Length)
        {
            livesDisplay.sprite = lifeSprites[lives];
        }
        else
        {
            Debug.LogError("Índice de sprites de vidas fuera de rango!");
        }
    }

    void UpdateCountdownDisplay(int seconds)
    {
        int index = 3 - seconds; 
        if (index >= 0 && index < countdownSprites.Length)
        {
            countdownDisplay.sprite = countdownSprites[index];
        }
        else
        {
            Debug.LogError("Índice de countdown fuera de rango!");
        }
    }

}
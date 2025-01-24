using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [Header("Configuración de Efecto")]
    public Image damageOverlay; // Debe ser un UI Image
    public float fadeDuration = 1f; // Tiempo para desaparecer el efecto
    public Color damageHealthColor = new Color(1, 0, 0, 0.3f); // Rojo semitransparente
    public Color damageShieldColor = new Color(0, 175, 255, 0.3f); // Azul cián semitransparente
    public AudioClip damageShieldSound; // Sonido cuando el escudo recibe daño
    public AudioClip damageHealthSound; // Sonido cuando la salud recibe daño
    private AudioSource audioSource;

    private Coroutine fadeCoroutine;

    void Start()
    {
        // Asegurarse que el overlay está invisible al inicio
        if (damageOverlay != null)
            damageOverlay.color = Color.clear;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TriggerHealthDamageEffect()
    {
        if (damageOverlay == null) return;

        // Detener fade anterior si está activo
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        Debug.Log("Damage effect triggering");
        // Reiniciar color y empezar fade
        damageOverlay.color = damageHealthColor;
        // reproduce sound
        audioSource.PlayOneShot(damageHealthSound);

        fadeCoroutine = StartCoroutine(FadeOut());
    }

    public void TriggerShieldDamageEffect()
    {
        if (damageOverlay == null) return;
        // Detener fade anterior si está activo
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        Debug.Log("Damage effect triggering");
        // Reiniciar color y empezar fade
        damageOverlay.color = damageShieldColor;
        // reproduce sound
        audioSource.PlayOneShot(damageShieldSound);
        fadeCoroutine = StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color startColor = damageOverlay.color;

        while (elapsedTime < fadeDuration)
        {
            damageOverlay.color = Color.Lerp(startColor, Color.clear, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        damageOverlay.color = Color.clear;
    }
}
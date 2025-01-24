using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [Header("Configuración de Efecto")]
    public Image damageOverlay; // Debe ser un UI Image
    public float fadeDuration = 1f; // Tiempo para desaparecer el efecto
    public Color damageColor = new Color(1, 0, 0, 0.3f); // Rojo semitransparente

    private Coroutine fadeCoroutine;

    void Start()
    {
        // Asegurarse que el overlay está invisible al inicio
        if (damageOverlay != null)
            damageOverlay.color = Color.clear;
    }

    public void TriggerDamageEffect()
    {
        if (damageOverlay == null) return;

        // Detener fade anterior si está activo
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        Debug.Log("Damage effect triggering");
        // Reiniciar color y empezar fade
        damageOverlay.color = damageColor;
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
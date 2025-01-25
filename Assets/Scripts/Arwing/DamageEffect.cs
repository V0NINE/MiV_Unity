using System.Runtime.CompilerServices;
using UnityEngine;
    using UnityEngine.UI;

    public class DamageEffect : MonoBehaviour
    {
        [Header("Configuración de Efecto")]
        public Image damageOverlay; // Debe ser un UI Image
        public float fadeDuration = 1f; // Tiempo para desaparecer el efecto
        public Color damageHealthColor = new Color(1, 0, 0, 0.3f); // Rojo semitransparente
        public Color damageShieldColor = new Color(0, 175, 255, 0.3f); // Azul cián semitransparente

        private Coroutine fadeCoroutine;

    private AudioManager audioManager;

    void Start()
        {
            // Asegurarse que el overlay está invisible al inicio
            if (damageOverlay != null)
                damageOverlay.color = Color.clear;

            audioManager = FindFirstObjectByType<AudioManager>();

    }

        public void TriggerHealthDamageEffect(Vector3 impactPoint)
        {
            if (damageOverlay == null) return;

            // Detener fade anterior si está activo
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            Debug.Log("Damage effect triggering");
            // Reiniciar color y empezar fade
            damageOverlay.color = damageHealthColor;


            EffectsManager.Instance.PlayDamageEffect(impactPoint, false); // Daño a la salud

        // reproduce sound
        audioManager.PlayHealthDamageSound();

            fadeCoroutine = StartCoroutine(FadeOut());
        }

        public void TriggerShieldDamageEffect(Vector3 impactPoint)
        {
            if (damageOverlay == null) return;
            // Detener fade anterior si está activo
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            Debug.Log("Damage effect triggering");
            // Reiniciar color y empezar fade
            damageOverlay.color = damageShieldColor;

        EffectsManager.Instance.PlayDamageEffect(impactPoint, true); // Daño al escudo

        // reproduce sound
        audioManager.PlayShieldDamageSound();

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
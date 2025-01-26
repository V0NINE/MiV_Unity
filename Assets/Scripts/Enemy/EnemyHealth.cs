using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    public int entityMaxHealth = 50;
    private int entityHealth;
    public float deathFadeDuration = 2f;

    private bool isDying = false;
    private List<Material> enemyMaterials = new List<Material>();
    private List<Color> originalColors = new List<Color>();

    private AudioManager audioManager;
    private LevelController levelController;

    void Start()
    {
        entityHealth = entityMaxHealth;
        audioManager = FindFirstObjectByType<AudioManager>();
        levelController = FindFirstObjectByType<LevelController>();

        // Obtener todos los materiales del enemigo
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                enemyMaterials.Add(mat);
                originalColors.Add(mat.color);
            }
        }
    }

    public void TakeDamage(int damage, Vector3 impactPoint)
    {
        if (isDying) return;

        entityHealth -= damage;

        EffectsManager.Instance.PlayDamageEffect(impactPoint, false); // Daño a la salud

        audioManager.PlayEnemyDamageSound();

        if (entityHealth <= 0)
        {
            levelController.OnEnemyKilled();
            audioManager.PlayEnemyDeathSound();
            StartCoroutine(DeathAnimation());
        }
    }

    private IEnumerator DeathAnimation()
    {
        isDying = true;

        float timer = 0f;

        // Desactivar componentes no necesarios durante la muerte
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // Opcional: Desactivar movimiento o IA
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) script.enabled = false;
        }

        while (timer < deathFadeDuration)
        {
            float progress = timer / deathFadeDuration;

            // Actualizar color de todos los materiales
            for (int i = 0; i < enemyMaterials.Count; i++)
            {
                Color targetColor = Color.Lerp(originalColors[i], Color.black, progress);
                enemyMaterials[i].color = Color.Lerp(enemyMaterials[i].color, targetColor, Time.deltaTime * 10f);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Asegurar color final completamente negro
        foreach (Material mat in enemyMaterials)
        {
            mat.color = Color.black;
        }

        audioManager.PlayEnemyExplosionSound();

        EffectsManager.Instance.PlayExplosion(transform.position, false); // Explosión enemiga

        Destroy(gameObject, 1.5f);
    }

    // Opcional: Restaurar colores originales si el objeto es reciclado
    void OnDestroy()
    {
        for (int i = 0; i < enemyMaterials.Count; i++)
        {
            if (enemyMaterials[i] != null)
            {
                enemyMaterials[i].color = originalColors[i];
            }
        }
    }
}
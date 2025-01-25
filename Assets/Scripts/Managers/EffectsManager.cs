using UnityEngine;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;

    [System.Serializable]
    public class EffectPool
    {
        public GameObject prefab;
        public Transform parentContainer;
        [HideInInspector] public Queue<GameObject> pool = new Queue<GameObject>();
    }

    [Header("Player Effects")]
    public EffectPool boostEffect;
    public EffectPool healthDamageEffect;
    public EffectPool shieldDamageEffect;

    [Header("Explosion Effects")]
    public EffectPool bombExplosion;
    public EffectPool enemyExplosion;

    [Header("Boost Components")]
    public Transform[] boostSpawnPoints;
    public EffectPool smallBoostEffect;

    [Header("Configuration")]
    public float defaultEffectDuration = 2f;
    public float boostEffectSizeMultiplier = 1f;

    private Dictionary<GameObject, EffectPool> activeEffectsMap = new Dictionary<GameObject, EffectPool>();
    private List<GameObject> activeBoostEffects = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAllPools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeAllPools()
    {
        InitializePool(boostEffect);
        InitializePool(smallBoostEffect);
        InitializePool(healthDamageEffect);
        InitializePool(shieldDamageEffect);
        InitializePool(bombExplosion);
        InitializePool(enemyExplosion);
    }

    void InitializePool(EffectPool pool)
    {
        // Vacío - Los objetos se crearán bajo demanda
    }

    public void PlayBoostEffect(bool isBigBoost)
    {
        foreach (Transform spawnPoint in boostSpawnPoints)
        {
            EffectPool poolToUse = isBigBoost ? boostEffect : smallBoostEffect;
            GameObject effect = GetEffectFromPool(poolToUse);

            effect.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            effect.transform.localScale = Vector3.one * boostEffectSizeMultiplier;
            effect.transform.SetParent(spawnPoint);

            effect.SetActive(true);
            activeBoostEffects.Add(effect);
            StartCoroutine(ReturnEffectAfterTime(poolToUse, effect, defaultEffectDuration));
        }
    }

    public void StopBoostEffects()
    {
        foreach (GameObject effect in activeBoostEffects)
        {
            if (effect != null && effect.activeSelf)
            {
                ReturnEffectToPool(boostEffect, effect);
                ReturnEffectToPool(smallBoostEffect, effect);
            }
        }
        activeBoostEffects.Clear();
    }

    public void PlayDamageEffect(Vector3 position, bool isShieldDamage)
    {
        EffectPool poolToUse = isShieldDamage ? shieldDamageEffect : healthDamageEffect;
        PlayEffectAtPosition(poolToUse, position);
    }

    public void PlayExplosion(Vector3 position, bool isBomb)
    {
        EffectPool poolToUse = isBomb ? bombExplosion : enemyExplosion;
        PlayEffectAtPosition(poolToUse, position);
    }

   

    private void PlayEffectAtPosition(EffectPool pool, Vector3 position)
    {
        GameObject effect = GetEffectFromPool(pool);
        effect.transform.position = position;
        effect.SetActive(true);
        StartCoroutine(ReturnEffectAfterTime(pool, effect, defaultEffectDuration));
    }

    private GameObject GetEffectFromPool(EffectPool pool)
    {
        // Buscar primero en objetos ya creados pero inactivos
        foreach (GameObject obj in pool.pool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                activeEffectsMap[obj] = pool;
                return obj;
            }
        }

        // Si no hay disponibles, crear nuevo
        GameObject newObj = Instantiate(pool.prefab, pool.parentContainer);
        activeEffectsMap[newObj] = pool;
        return newObj;
    }

    private void ReturnEffectToPool(EffectPool pool, GameObject effect)
    {
        if (effect == null) return;

        effect.SetActive(false);
        effect.transform.SetParent(pool.parentContainer);
        ResetParticleSystem(effect);

        // No añadir a la cola, se maneja por estado active/inactive
        activeEffectsMap.Remove(effect);
    }

    private void ResetParticleSystem(GameObject effect)
    {
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
            ps.Clear();
        }
    }

    private System.Collections.IEnumerator ReturnEffectAfterTime(EffectPool pool, GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnEffectToPool(pool, effect);
    }
}
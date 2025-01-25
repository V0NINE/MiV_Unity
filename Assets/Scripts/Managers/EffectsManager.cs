using UnityEngine;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;

    [System.Serializable]
    public class EffectPool
    {
        public GameObject prefab;
        public int poolSize = 10;
        public Transform parentContainer;
        [HideInInspector] public Queue<GameObject> pool = new Queue<GameObject>();
    }

    [Header("Player Effects")]
    public EffectPool boostEffect;
    public EffectPool bulletImpactEffect;
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
        InitializePool(bulletImpactEffect);
        InitializePool(healthDamageEffect);
        InitializePool(shieldDamageEffect);
        InitializePool(bombExplosion);
        InitializePool(enemyExplosion);
    }

    void InitializePool(EffectPool pool)
    {
        for (int i = 0; i < pool.poolSize; i++)
        {
            GameObject obj = Instantiate(pool.prefab, pool.parentContainer);
            obj.SetActive(false);
            pool.pool.Enqueue(obj);
        }
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

    public void PlayBulletImpact(Vector3 position, Transform parent = null)
    {
        GameObject effect = GetEffectFromPool(bulletImpactEffect);
        effect.transform.position = position;
        if (parent != null) effect.transform.SetParent(parent);
        effect.SetActive(true);
        StartCoroutine(ReturnEffectAfterTime(bulletImpactEffect, effect, defaultEffectDuration));
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
        if (pool.pool.Count > 0)
        {
            GameObject obj = pool.pool.Dequeue();
            activeEffectsMap[obj] = pool;
            return obj;
        }

        // Dynamic pool growth
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

        pool.pool.Enqueue(effect);
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
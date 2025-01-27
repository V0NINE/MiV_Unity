using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Música")]
    public AudioClip backgroundMusic;
    private AudioSource musicSource;
    public bool musicLoop = true;

    [Header("Bomb")]
    public AudioClip bombSound;
    private AudioSource bombSource;
    public AudioClip bombLaunchSound;
    private AudioSource bombLaunchSource;

    [Header("Boost")]
    public AudioClip boostSound;
    private AudioSource boostSource;
    public AudioClip boostFadeSound;
    private AudioSource boostFadeSource;

    [Header("Laser")]
    public AudioClip laserSound;
    private AudioSource laserSource;
    private Queue<AudioSource> laserAudioPool = new();

    [Header("Shield")]
    public AudioClip shieldDamage;
    private Queue<AudioSource> shieldDamagePool = new();
    public AudioClip shieldBreakSound;
    private AudioSource shieldBreakSource;

    [Header("Health")]
    public AudioClip healthDamageSound;
    private Queue<AudioSource> healthDamagePool = new();

    [Header("Game Over")]
    public AudioClip gameOverSound;
    private AudioSource gameOverSource;

    [Header("Rocket League Countdown")]
    public AudioClip countdownSound;
    private AudioSource countdownSource;

    [Header("Enemy")]
    public AudioClip enemyDamageSound;
    private Queue<AudioSource> enemyDamagePool = new();
    public AudioClip enemyExplosionSound;
    private AudioSource enemyExplosionSource;
    public AudioClip[] deathSounds;
    private AudioSource enemyDeathSource;
    private AudioSource enemyShotSource;
    public AudioClip enemyShotSound;
    private AudioSource enemySpottedSource;
    public AudioClip enemySpottedSound;

    [Header("Portal")]
    public AudioClip netherPortalSound;
    private AudioSource netherPortalSource;
    public AudioClip netherPortalEnteredSound;
    private AudioSource netherPortalEnteredSource;

    private const int POOL_SIZE = 5;


    [Header("Configuración")]
    [Range(0, 1)] public float musicVolume = 0.5f;
    [Range(0, 1)] public float laserVolume = 0.3f;
    [Range(0, 1)] public float boostVolume = 0.5f;
    [Range(0, 1)] public float boostFadeVolume = 0.5f;
    [Range(0, 1)] public float bombVolume = 0.5f;
    [Range(0, 1)] public float bombLaunchVolume = 0.5f;
    [Range(0, 1)] public float shieldDamageVolume = 0.5f;
    [Range(0, 1)] public float shieldBreakVolume = 0.5f;
    [Range(0, 1)] public float healthDamageVolume = 0.5f;
    [Range(0, 1)] public float gameOverVolume = 0.5f;
    [Range(0, 1)] public float countdownVolume = 0.5f;
    [Range(0, 1)] public float enemyDamageVolume = 0.5f;
    [Range(0, 1)] public float enemyExplosionVolume = 0.5f;
    [Range(0, 1)] public float enemyDeathVolume = 0.5f;
    [Range(0, 1)] public float enemyShotVolume = 0.5f;
    [Range(0, 1)] public float enemySpottedVolume = 0.5f;
    [Range(0, 1)] public float netherPortalVolume = 0.5f;
    [Range(0, 1)] public float netherPortalEnteredVolume = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAllAudioSources();
            InitializeLaserSound();
            InitializeShieldDamageSound(); 
            InitializeHealthDamageSound();
            InitializeEnemyDamageSound();
            InitializeNetherPortalSound();
            IntializeNetherPortalEntederSound();
            PlayMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeLaserSound()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = laserSound;
            source.volume = laserVolume;
            source.playOnAwake = false;
            laserAudioPool.Enqueue(source);
        }
    }

    void InitializeShieldDamageSound()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = shieldDamage;
            source.volume = shieldDamageVolume;
            source.playOnAwake = false;
            shieldDamagePool.Enqueue(source);
        }
    }

    void InitializeHealthDamageSound()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = healthDamageSound;
            source.volume = healthDamageVolume;
            source.playOnAwake = false;
            healthDamagePool.Enqueue(source);
        }
    }

    void InitializeEnemyDamageSound()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = enemyDamageSound;
            source.volume = enemyDamageVolume;
            source.playOnAwake = false;
            enemyDamagePool.Enqueue(source);
        }
    }

    public void InitializeNetherPortalSound()
    {
        netherPortalSource = CreateAudioSource(netherPortalSound, netherPortalVolume);
        netherPortalSource.loop = true;
        netherPortalSource.playOnAwake = false;
    }

    public void IntializeNetherPortalEntederSound()
    {
        netherPortalEnteredSource = CreateAudioSource(netherPortalEnteredSound, netherPortalEnteredVolume);
    }

    public void StopNetherSound()
    {
        if (netherPortalSource != null)
        {
            netherPortalSource.Stop();
        }
    }

    private void InitializeAllAudioSources()
    {
        // Música
        musicSource = CreateAudioSource(backgroundMusic, musicVolume, musicLoop);

        // Bomb
        bombSource = CreateAudioSource(bombSound, bombVolume);
        bombLaunchSource = CreateAudioSource(bombLaunchSound, bombLaunchVolume);

        // Boost
        boostSource = CreateAudioSource(boostSound, boostVolume);
        boostFadeSource = CreateAudioSource(boostFadeSound, boostFadeVolume);

        // Laser
        laserSource = CreateAudioSource(laserSound, laserVolume);

        // Shield
        shieldBreakSource = CreateAudioSource(shieldBreakSound, shieldBreakVolume);


        // Game Over
        gameOverSource = CreateAudioSource(gameOverSound, gameOverVolume);

        // Countdown
        countdownSource = CreateAudioSource(countdownSound, countdownVolume);

        // Enemy
        enemyExplosionSource = CreateAudioSource(enemyExplosionSound, enemyExplosionVolume);
        enemyDeathSource = CreateAudioSource(null, enemyDeathVolume);
        enemyShotSource = CreateAudioSource(enemyShotSound, enemyShotVolume);
        enemySpottedSource = CreateAudioSource(enemySpottedSound, enemySpottedVolume);
    }

    private AudioSource CreateAudioSource(AudioClip clip, float volume, bool loop = false)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.playOnAwake = false;
        return source;
    }

    public void PlayShieldDamageSound()
    {
        PlayWithPool(shieldDamagePool, shieldDamage, shieldDamageVolume);
    }

    public void PlayHealthDamageSound()
    {
        PlayWithPool(healthDamagePool, healthDamageSound, healthDamageVolume);
    }

    public void PlayEnemyDamageSound()
    {
        PlayWithPool(enemyDamagePool, enemyDamageSound, enemyDamageVolume);
    }

    private void PlayWithPool(Queue<AudioSource> pool, AudioClip clip, float volume)
    {
        AudioSource source = GetAvailableSource(pool, clip);
        source.volume = volume;
        source.Play();
        StartCoroutine(ReturnToPool(pool, source, clip.length));
    }

    private AudioSource GetAvailableSource(Queue<AudioSource> pool, AudioClip clip)
    {
        if (pool.Count > 0) return pool.Dequeue();

        // Si el pool está vacío, crear nueva fuente dinámicamente
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = clip;
        newSource.playOnAwake = false;
        return newSource;
    }

    private IEnumerator ReturnToPool(Queue<AudioSource> pool, AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        source.Stop();
        pool.Enqueue(source);
    }

    public void PlayLaserSound()
    {
        if (laserAudioPool.Count > 0)
        {
            AudioSource source = laserAudioPool.Dequeue();
            source.Play();
            StartCoroutine(ReturnToLaserPool(source));
        }
        else
        {
            // Crear nuevo AudioSource dinámicamente si se necesita
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = laserSound;
            newSource.volume = laserVolume;
            newSource.Play();
            StartCoroutine(ReturnToLaserPool(newSource));
        }
    }

    private IEnumerator ReturnToLaserPool(AudioSource source)
    {
        yield return new WaitForSeconds(laserSound.length);
        source.Stop();
        laserAudioPool.Enqueue(source);
    }

    public void PlayMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

   

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PlayBoombSound() => PlaySound(bombSource);
    public void PlayBombLaunchSound() => PlaySound(bombLaunchSource);
    public void PlayBoostSound() => PlaySound(boostSource);
    public void StopBoostSound() => StopSound(boostSource);
    public void PlayBoostFadeSound() => PlaySound(boostFadeSource);
    public void PlayShieldBreakSound() => PlaySound(shieldBreakSource);
    public void PlayGameOverSound() => PlaySound(gameOverSource);
    public void PlayCountdownSound() => PlaySound(countdownSource);
    public void PlayEnemyExplosionSound() => PlaySound(enemyExplosionSource);

    public void PlayNetherSound() => PlaySound(netherPortalSource);
    public void PlayNetherEnteredSound() => PlaySound(netherPortalEnteredSource);

    public void PlayEnemyDeathSound()
    {
        if (enemyDeathSource != null && deathSounds != null && deathSounds.Length > 0)
        {
            enemyDeathSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
            PlaySound(enemyDeathSource);
        }
    }

    public void PlayEnemyShotSound() => PlaySound(enemyShotSource);

    public void PlayEnemySpottedSound() => PlaySound(enemySpottedSource);

    private void PlaySound(AudioSource source)
    {
        if (source != null && source.clip != null)
        {
            source.Play();
        }
    }

    private void StopSound(AudioSource source)
    {
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }

    // Métodos para actualizar volúmenes en tiempo real
    public void UpdateMusicVolume(float volume) => UpdateVolume(ref musicVolume, musicSource, volume);
    public void UpdateLaserVolume(float volume) => UpdateVolume(ref laserVolume, laserSource, volume);
    public void UpdateBoostVolume(float volume) => UpdateVolume(ref boostVolume, boostSource, volume);
    public void UpdateBoostFadeVolume(float volume) => UpdateVolume(ref boostFadeVolume, boostFadeSource, volume);
    public void UpdateBombVolume(float volume) => UpdateVolume(ref bombVolume, bombSource, volume);
    public void UpdateBombLaunchVolume(float volume) => UpdateVolume(ref bombLaunchVolume, bombLaunchSource, volume);
    public void UpdateShieldBreakVolume(float volume) => UpdateVolume(ref shieldBreakVolume, shieldBreakSource, volume);
    public void UpdateGameOverVolume(float volume) => UpdateVolume(ref gameOverVolume, gameOverSource, volume);
    public void UpdateCountdownVolume(float volume) => UpdateVolume(ref countdownVolume, countdownSource, volume);
    public void UpdateEnemyExplosionVolume(float volume) => UpdateVolume(ref enemyExplosionVolume, enemyExplosionSource, volume);
    public void UpdateEnemyDeathVolume(float volume) => UpdateVolume(ref enemyDeathVolume, enemyDeathSource, volume);

    public void UpdateEnemySpottedVolume(float volume) => UpdateVolume(ref enemySpottedVolume, enemySpottedSource, volume);

    public void UpdateNetherPortalVolume(float volume) => UpdateVolume(ref netherPortalVolume, netherPortalSource, volume);
    public void UpdateNetherPortalEnteredVolume(float volume) => UpdateVolume(ref netherPortalEnteredVolume, netherPortalEnteredSource, volume);

    public void UpdateShieldDamageVolume(float volume)
    {
        shieldDamageVolume = Mathf.Clamp01(volume);
        UpdatePoolVolume(shieldDamagePool, shieldDamageVolume);
    }

    public void UpdateHealthDamageVolume(float volume)
    {
        healthDamageVolume = Mathf.Clamp01(volume);
        UpdatePoolVolume(healthDamagePool, healthDamageVolume);
    }

    public void UpdateEnemyDamageVolume(float volume)
    {
        enemyDamageVolume = Mathf.Clamp01(volume);
        UpdatePoolVolume(enemyDamagePool, enemyDamageVolume);
    }

    private void UpdatePoolVolume(Queue<AudioSource> pool, float newVolume)
    {
        foreach (var source in pool)
        {
            if (source != null) source.volume = newVolume;
        }
    }

    private void UpdateVolume(ref float configVolume, AudioSource source, float newVolume)
    {
        configVolume = Mathf.Clamp01(newVolume);
        if (source != null) source.volume = configVolume;
    }
}
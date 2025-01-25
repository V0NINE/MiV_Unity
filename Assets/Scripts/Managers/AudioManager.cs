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

    [Header("Shield")]
    public AudioClip shieldDamage;
    private AudioSource shieldDamageSource;
    public AudioClip shieldBreakSound;
    private AudioSource shieldBreakSource;

    [Header("Health")]
    public AudioClip healthDamageSound;
    private AudioSource healthDamageSource;

    [Header("Game Over")]
    public AudioClip gameOverSound;
    private AudioSource gameOverSource;

    [Header("Rocket League Countdown")]
    public AudioClip countdownSound;
    private AudioSource countdownSource;

    [Header("Enemy")]
    public AudioClip enemyDamageSound;
    private AudioSource enemyDamageSource;
    public AudioClip enemyExplosionSound;
    private AudioSource enemyExplosionSource;
    public AudioClip[] deathSounds;
    private AudioSource enemyDeathSource;
    private AudioSource enemyShotSource;
    public AudioClip enemyShotSound;

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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAllAudioSources();
            PlayMusic();
        }
        else
        {
            Destroy(gameObject);
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
        shieldDamageSource = CreateAudioSource(shieldDamage, shieldDamageVolume);
        shieldBreakSource = CreateAudioSource(shieldBreakSound, shieldBreakVolume);

        // Health
        healthDamageSource = CreateAudioSource(healthDamageSound, healthDamageVolume);

        // Game Over
        gameOverSource = CreateAudioSource(gameOverSound, gameOverVolume);

        // Countdown
        countdownSource = CreateAudioSource(countdownSound, countdownVolume);

        // Enemy
        enemyDamageSource = CreateAudioSource(enemyDamageSound, enemyDamageVolume);
        enemyExplosionSource = CreateAudioSource(enemyExplosionSound, enemyExplosionVolume);
        enemyDeathSource = CreateAudioSource(null, enemyDeathVolume);
        enemyShotSource = CreateAudioSource(enemyShotSound, enemyShotVolume);
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
    public void PlayLaserSound() => PlaySound(laserSource);
    public void PlayShieldBreakSound() => PlaySound(shieldBreakSource);
    public void PlayShieldDamageSound() => PlaySound(shieldDamageSource);
    public void PlayGameOverSound() => PlaySound(gameOverSource);
    public void PlayHealthDamageSound() => PlaySound(healthDamageSource);
    public void PlayCountdownSound() => PlaySound(countdownSource);
    public void PlayEnemyDamageSound() => PlaySound(enemyDamageSource);
    public void PlayEnemyExplosionSound() => PlaySound(enemyExplosionSource);

    public void PlayEnemyDeathSound()
    {
        if (enemyDeathSource != null && deathSounds != null && deathSounds.Length > 0)
        {
            enemyDeathSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
            PlaySound(enemyDeathSource);
        }
    }

    public void PlayEnemyShotSound() => PlaySound(enemyShotSource);

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
    public void UpdateShieldDamageVolume(float volume) => UpdateVolume(ref shieldDamageVolume, shieldDamageSource, volume);
    public void UpdateShieldBreakVolume(float volume) => UpdateVolume(ref shieldBreakVolume, shieldBreakSource, volume);
    public void UpdateHealthDamageVolume(float volume) => UpdateVolume(ref healthDamageVolume, healthDamageSource, volume);
    public void UpdateGameOverVolume(float volume) => UpdateVolume(ref gameOverVolume, gameOverSource, volume);
    public void UpdateCountdownVolume(float volume) => UpdateVolume(ref countdownVolume, countdownSource, volume);
    public void UpdateEnemyDamageVolume(float volume) => UpdateVolume(ref enemyDamageVolume, enemyDamageSource, volume);
    public void UpdateEnemyExplosionVolume(float volume) => UpdateVolume(ref enemyExplosionVolume, enemyExplosionSource, volume);
    public void UpdateEnemyDeathVolume(float volume) => UpdateVolume(ref enemyDeathVolume, enemyDeathSource, volume);

    private void UpdateVolume(ref float configVolume, AudioSource source, float newVolume)
    {
        configVolume = Mathf.Clamp01(newVolume);
        if (source != null) source.volume = configVolume;
    }
}
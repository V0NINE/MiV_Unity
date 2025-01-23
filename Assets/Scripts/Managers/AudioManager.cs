using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Música")]
    public AudioClip backgroundMusic;
    private AudioSource musicSource;
    [Header("Configuración")]
    [Range(0, 1)] public float musicVolume = 0.5f;
    public bool musicLoop = true;

    [Header("Bomb")]
    public AudioClip bombSound;
    private AudioSource bombSource;

    [Header("Boost")]
    public AudioClip boostSound;
    private AudioSource boostSource;
    public AudioClip boostFadeSound;
    private AudioSource boostFadeSource;

    [Header("Laser")]
    public AudioClip laserSound;
    private AudioSource laserSource;
    [Header("Configuración")]
    public float laserVolume = 0.3f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

	SetBackgroundMusic();
	SetBoombSound();
	SetBoostSound();
	SetBoostFadeSound();
	SetLaserSound();

        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopMusic(float fadeDuration = 1f)
    {
        //StartCoroutine(FadeOutMusic(fadeDuration));
        musicSource.Stop();
    }

    public void PlayBoombSound()
    {
	if(bombSource != null && bombSound != null)
	    bombSource.Play();
    }

    public void PlayBoostSound()
    {
	if(boostSource != null && boostSound != null)
	    boostSource.Play();
    }

    public void StopBoostSound()
    {
	if(boostSource != null && boostSound != null)
	    boostSource.Stop();
    }

    public void PlayBoostFadeSound()
    {
	if(boostFadeSource != null && boostFadeSound != null)
	    boostFadeSource.Play();
    }

    public void PlayLaserSound()
    {
	if(laserSource != null && laserSound != null)
	    laserSource.Play();
    }
   
    void SetBackgroundMusic()
    {
        // Configurar fuente de música
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.volume = musicVolume;
        musicSource.loop = musicLoop;
    }

    void SetBoombSound()
    {
	bombSource = gameObject.AddComponent<AudioSource>();
	bombSource.clip = bombSound;
	bombSource.playOnAwake = false;
    }

    void SetBoostSound()
    {
	boostSource = gameObject.AddComponent<AudioSource>();
	boostSource.clip = boostSound;
	boostSource.playOnAwake = false;
    }

    void SetBoostFadeSound()
    {
	boostFadeSource = gameObject.AddComponent<AudioSource>();
	boostFadeSource.clip = boostFadeSound;
	boostFadeSource.playOnAwake = false;
    }

    void SetLaserSound()
    {
	laserSource = gameObject.AddComponent<AudioSource>();
	laserSource.clip = laserSound;
	laserSource.volume = laserVolume;
	laserSource.playOnAwake = false;
    }
}

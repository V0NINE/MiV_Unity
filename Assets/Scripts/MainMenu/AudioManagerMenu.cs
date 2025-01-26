using UnityEngine;

public class AudioManagerMenu : MonoBehaviour
{
    public static AudioManagerMenu Instance;

    [Header("Música")]
    public AudioClip mainMusic;
    [Range(0, 1)] public float musicVolume = 1;

    [Header("Efecto seleccion de botón")]
    public AudioClip buttonSelect;
    [Range(0, 1)] public float buttonSelectVolume = 1;

    private AudioSource musicSource;
    private AudioSource buttonSelectSource;

    void Awake()
    {
        // Implementar patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            

            // Configurar AudioSource
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.clip = mainMusic;

            buttonSelectSource = gameObject.AddComponent<AudioSource>();
            buttonSelectSource.volume = buttonSelectVolume;
            buttonSelectSource.clip = buttonSelect;
            buttonSelectSource.loop = false;


            PlayMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayButtonSelect()
    {
        musicSource.PlayOneShot(buttonSelect, buttonSelectVolume);
    }


    public void UpdateButtonSelectVolume(float newVolume)
    {
        buttonSelectVolume = newVolume;

    }

    public void UpdateVolume(float newVolume)
    {
        musicVolume = newVolume;
        musicSource.volume = musicVolume;
    }
}
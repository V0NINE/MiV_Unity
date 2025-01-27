using UnityEngine;

public class AudioManagerFinal : MonoBehaviour
{
    public static AudioManagerFinal Instance;

    [Header("Efecto seleccion de botón")]
    public AudioClip buttonSelect;
    [Range(0, 1)] public float buttonSelectVolume = 1;

    private AudioSource buttonSelectSource;

    void Awake()
    {
        // Implementar patrón Singleton
        if (Instance == null)
        {
            Instance = this;

            buttonSelectSource = gameObject.AddComponent<AudioSource>();
            buttonSelectSource.volume = buttonSelectVolume;
            buttonSelectSource.clip = buttonSelect;
            buttonSelectSource.loop = false;


        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonSelect()
    {
        buttonSelectSource.PlayOneShot(buttonSelect, buttonSelectVolume);
    }


    public void UpdateButtonSelectVolume(float newVolume)
    {
        buttonSelectVolume = newVolume;

    }

}
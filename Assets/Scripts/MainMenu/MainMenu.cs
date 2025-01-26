using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject creditsPanel;

    [Header("Botones")]
    [SerializeField] Button playButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button returnButtonOptions;
    [SerializeField] Button returnButtonCredits;

    [Header("Configuración")]
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle fullscreenToggle;

    private Canvas mainMenu;

    private AudioManagerMenu audioManager;

    void Start()
    {
        // Obtener referencia al AudioManager
        audioManager = FindFirstObjectByType<AudioManagerMenu>();
        audioManager.PlayMusic();

        mainMenu = GetComponent<Canvas>();
        mainMenu.enabled = true;
        Debug.Log("Main Menu started!");
        // Configuración inicial
        ShowPanel(mainPanel);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Asignar eventos
        playButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(() => ShowPanel(optionsPanel));
        creditsButton.onClick.AddListener(() => ShowPanel(creditsPanel));
        quitButton.onClick.AddListener(QuitGame);
        returnButtonOptions.onClick.AddListener(ReturnToMainMenu);
        returnButtonCredits.onClick.AddListener(ReturnToMainMenu);

        // Configurar opciones
        volumeSlider.onValueChanged.AddListener(SetVolume);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // Cargar valores guardados
        LoadSavedSettings();
    }

    void ShowPanel(GameObject panel)
    {
        audioManager.PlayButtonSelect();
        Debug.Log("Showing panel: " + panel.name);
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        panel.SetActive(true);
    }

    public void StartGame()
    {
        audioManager.PlayButtonSelect();
        Debug.Log("Starting game...");
        mainMenu.enabled = false;
        
        audioManager.StopMusic();
        
        SceneManager.LoadScene(1); 

    }

    void QuitGame()
    {
        audioManager.PlayButtonSelect();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    void LoadSavedSettings()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    // Para el botón de volver (agregar a los paneles hijos)
    public void ReturnToMainMenu()
    {
        audioManager.PlayButtonSelect();
        ShowPanel(mainPanel);
    }
}
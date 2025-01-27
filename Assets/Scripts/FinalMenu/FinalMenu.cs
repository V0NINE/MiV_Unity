using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalMenu : MonoBehaviour
{

    [Header("Botones")]
    [SerializeField] Button restartButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button quitButton;

    private Canvas finalMenu;
    [Header("Paneles")]
    [SerializeField] GameObject mainPanel;

    private AudioManagerFinal audioManager;

    void Start()
    {
        // Obtener referencia al AudioManager
        audioManager = FindFirstObjectByType<AudioManagerFinal>();
   
        mainPanel.SetActive(true);
        finalMenu = GetComponent<Canvas>();
        finalMenu.enabled = true;
        Debug.Log("Final Menu started!");
        // Configuración inicial
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Asignar eventos
        restartButton.onClick.AddListener(StartGame);
        menuButton.onClick.AddListener(ReturnToMainMenu);
        quitButton.onClick.AddListener(QuitGame);
    }


    public void StartGame()
    {
        audioManager.PlayButtonSelect();
        Debug.Log("Starting game...");
        mainPanel.SetActive(false);
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

 
    // Para el botón de volver (agregar a los paneles hijos)
    public void ReturnToMainMenu()
    {
        audioManager.PlayButtonSelect();
        SceneManager.LoadScene(0);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class LifeUIManager : MonoBehaviour
{
    [Header("Sprites de Vidas")]
    public Sprite twoLivesSprite;
    public Sprite oneLifeSprite;
    public Sprite zeroLivesSprite;

    public Image livesImage;
    private ArwingHealth playerHealth;

    void Start()
    {
        livesImage = GetComponent<Image>();
        playerHealth = FindFirstObjectByType<ArwingHealth>();

        if (playerHealth != null)
        {
            // Suscribirse al evento
            playerHealth.OnLivesChanged += UpdateLivesUI;
            UpdateLivesUI(); // Actualizar al inicio
        }
        else
        {
            Debug.LogError("No se encontró ArwingHealth en la escena!");
        }
    }

    void UpdateLivesUI()
    {
        switch (playerHealth.currentLives)
        {
            case 2:
                Debug.Log("Updating lives UI to 2 lives");
                livesImage.sprite = twoLivesSprite;
                break;
            case 1:
                Debug.Log("Updating lives UI to 1 life");
                livesImage.sprite = oneLifeSprite;
                break;
            case 0:
                Debug.Log("Updating lives UI to 0 lives");
                livesImage.sprite = zeroLivesSprite;
                break;
        }
    }

    void OnDestroy()
    {
        // Desuscribirse para evitar errores
        if (playerHealth != null)
        {
            playerHealth.OnLivesChanged -= UpdateLivesUI;
        }
    }
}
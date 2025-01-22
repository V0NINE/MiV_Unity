using UnityEngine;

public class BoostController : MonoBehaviour
{
    public Camera mainCamera;
    public float boostDuration = 1.0f;
    public float cameraFOVBoost = 65f;
    public float entitySpeedMultiplier = 2f;
    
    private float normalFOV;
    private float currentBoostTime;
    private bool isBoosting;

    // Entities gestionats pel controlador global EntityManager
    private EntityManager entityManager;

    void Start()
    {
        // FOV original de la càmera
        normalFOV = mainCamera.fieldOfView;

       	entityManager = FindFirstObjectByType<EntityManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartBoost();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndBoost();
        }

        // Gestiona el boost actiu
        if (isBoosting)
        {
            currentBoostTime += Time.deltaTime;

            // Si el boost supera la durada màxima, finalitza el boost 
            if (currentBoostTime >= boostDuration)
            {
                EndBoost();
            }
        }
    }

    void StartBoost()
    {
        isBoosting = true;
        currentBoostTime = 0f;

        mainCamera.fieldOfView = cameraFOVBoost;

        entityManager.SetEntitySpeedMultiplier(entitySpeedMultiplier);
    }

    void EndBoost()
    {
        isBoosting = false;

        mainCamera.fieldOfView = normalFOV;

        entityManager.SetEntitySpeedMultiplier(1f);
    }
}

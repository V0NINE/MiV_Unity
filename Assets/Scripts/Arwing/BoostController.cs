using UnityEngine;
using UnityEngine.UI;

public class BoostController : MonoBehaviour
{
    public Camera mainCamera;
    public float boostDuration = 2.0f;
    public float cameraFOVBoost = 65f;
    public float entitySpeedMultiplier = 2f;
    public float landSpeedMultiplier = 2f;
    
    private float normalFOV;
    private float currentBoostTime = 0f;
    private bool isBoosting;

    public Image boostBar; 

    // Entities gestionats pel controlador global EntityManager
    private EnemyManager entityManager;
    private Portal portal;

    private LandTextureScroller landScroller;

    private BoostEffect boostEffect;
    private AudioManager audioManager;

    void Start()
    {
        // FOV original de la càmera
        normalFOV = mainCamera.fieldOfView;

       	entityManager = FindFirstObjectByType<EnemyManager>();
	landScroller = FindFirstObjectByType<LandTextureScroller>();
        portal = FindFirstObjectByType<Portal>();

        boostEffect = GetComponent<BoostEffect>();
	audioManager = FindFirstObjectByType<AudioManager>();

	UpdateBoostUI();
    }

    void Update()
    {
        portal = FindFirstObjectByType<Portal>();
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
	    UpdateBoostUI();

            // Si el boost supera la durada màxima, finalitza el boost 
            if (currentBoostTime >= boostDuration)
            {
                EndBoost();
            }
        }
	else if(!isBoosting && currentBoostTime > 0)
	{
	    currentBoostTime -= Time.deltaTime * 1.2f;
	    UpdateBoostUI();
	}
    }

    void StartBoost()
    {
        isBoosting = true;

        mainCamera.fieldOfView = cameraFOVBoost;

        entityManager.SetEntitySpeedMultiplier(entitySpeedMultiplier);
        if (portal != null)
            portal.SetSpeedMultiplier(entitySpeedMultiplier);
        landScroller.SetScrollSpeedMultiplier(landSpeedMultiplier);

	boostEffect.StartBoostEffect();
	audioManager.PlayBoostSound();

    }

    void EndBoost()
    {
	audioManager.StopBoostSound();
	if(isBoosting)
	    audioManager.PlayBoostFadeSound();

        mainCamera.fieldOfView = normalFOV;

        entityManager.SetEntitySpeedMultiplier(1f);
        if (portal != null)
            portal.SetSpeedMultiplier(1f);
        landScroller.SetScrollSpeedMultiplier(1f);

	boostEffect.EndBoostEffect();

        isBoosting = false;
    }

    void UpdateBoostUI()
    {
	if(boostBar != null)
	    boostBar.fillAmount = 1 - (float)currentBoostTime/boostDuration;
    }
}

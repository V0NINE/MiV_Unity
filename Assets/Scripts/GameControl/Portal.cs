using UnityEngine;

public class Portal : MonoBehaviour
{

    private LevelController levelController;

    [SerializeField] float baseSpeed = 15f;
    [SerializeField] float initialZ = 68f;
    [SerializeField] float resetZ = -20f;
    private float speedMultiplier = 1f;
    private Vector3 movementAxis = Vector3.back;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        levelController = FindFirstObjectByType<LevelController>();
        audioManager.PlayNetherSound();
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
        MovePortal();
        CheckReset();
    }

    void MovePortal()
    {
        // Movimiento independiente de la rotación usando eje global
        transform.position += movementAxis * baseSpeed * speedMultiplier * Time.deltaTime;
    }

    void CheckReset()
    {
        if (transform.position.z <= resetZ)
        {
            RepositionPortal();
        }
    }

    void RepositionPortal()
    {
        // Conserva la posición X actual para posibles patrones laterales
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            initialZ
        );
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.StopNetherSound();
            levelController.OnPortalEntered();
        }
    }
}
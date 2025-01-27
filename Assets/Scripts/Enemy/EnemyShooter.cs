using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Configuraci�n de Disparo")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float shootingRange = 45f;

    private AudioManager audioManager;
    private Transform playerTransform;
    private float nextFireTime;

    [Header("Orientaci�n al Disparar")]
    public bool facePlayerWhenShooting = true;
    public float rotationSpeed = 50f;

    public float spawnForce = 10f;

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= shootingRange)
        {
            if (facePlayerWhenShooting)
            {
                RotateTowardsPlayer();
            }

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0; // Ignora altura para rotaci�n horizontal

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        // Asegura que la rotaci�n solo sea en Y
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void Shoot()
    {
        // Usa la rotaci�n actual del firePoint manteniendo la inclinaci�n correcta
        GameObject bullet = Instantiate(
            projectilePrefab,
            firePoint.position,
             firePoint.rotation * Quaternion.Euler(90, 0, 0)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Calcula direcci�n hacia el jugador (incluyendo altura)
            Vector3 direction = (playerTransform.position - firePoint.position).normalized;
            rb.AddForce(direction * spawnForce, ForceMode.Impulse);
        }
        audioManager.PlayEnemyShotSound();
    }
}
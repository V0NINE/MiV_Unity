using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float shootingRange = 45f;

    private AudioManager audioManager;
    private Transform playerTransform;
    private float nextFireTime;

    [Header("Orientación al Disparar")]
    public bool facePlayerWhenShooting = true;
    public float rotationSpeed = 5f;

    public float spawn_force = 10f;

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
        direction.y = 0; // Ignora altura para rotación horizontal

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        // Asegura que la rotación solo sea en Y
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void Shoot()
    {
        // Usa la rotación actual del firePoint manteniendo la inclinación correcta
        GameObject bullet = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation * Quaternion.Euler(90, 0, 0)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Dispara en la dirección del firePoint (que sigue al jugador)
            rb.AddForce(firePoint.forward * spawn_force, ForceMode.Impulse);
        }
        audioManager.PlayEnemyShotSound();
    }
}
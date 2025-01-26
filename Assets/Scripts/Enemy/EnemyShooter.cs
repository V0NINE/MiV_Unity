using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float shootingRange = 20f;

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
        // deactivate enemyaproximate script
        EnemyAproximate enemyAproximate = GetComponent<EnemyAproximate>();
        if (enemyAproximate != null)
        {
            enemyAproximate.enabled = false;
        }

        Vector3 direction = playerTransform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        // reactivate enemyaproximate script
        if (enemyAproximate != null)
        {
            enemyAproximate.enabled = true;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(firePoint.rotation.eulerAngles.x + 90f,
                                                      firePoint.rotation.eulerAngles.y,
                                                      0f));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null) rb.AddForce(firePoint.forward * spawn_force, ForceMode.Impulse);
        audioManager.PlayEnemyShotSound();
    }
}
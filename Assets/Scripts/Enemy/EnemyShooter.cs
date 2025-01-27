using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Configuraci�n de Disparo")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public float shootingRange = 50f;

    private AudioManager audioManager;
    private Transform playerTransform;
    private float nextFireTime;

    [Header("Orientaci�n al Disparar")]
    public bool facePlayerWhenShooting = true;
    public float rotationSpeed = 50f;
    public float rotationOffset = 1.2f;

    public float spawnForce = 25f;

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
        direction.x = direction.x + 5f;
        direction.z = direction.z + 5f;

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

        Vector3 direction = playerTransform.position - firePoint.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        firePoint.rotation = Quaternion.Euler(rotation.eulerAngles.x + rotationOffset, rotation.eulerAngles.y, firePoint.rotation.eulerAngles.z);

        // Usa la rotaci�n actual del firePoint manteniendo la inclinaci�n correcta
        GameObject bullet = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation * Quaternion.Euler(-90, 0, 0)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Dispara en la direcci�n del firePoint (que sigue al jugador)
            rb.AddForce(firePoint.transform.forward * spawnForce, ForceMode.Impulse);
        }
        audioManager.PlayEnemyShotSound();
    }
}
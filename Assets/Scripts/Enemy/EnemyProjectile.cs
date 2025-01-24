using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private const string ENEMY_TAG = "Enemy";

    public int damage = 10;
    public float speed = 30f; // Velocidad alta (30 unidades/segundo)
    private ArwingHealth playerHealth;
    public GameObject impactEffect; 
    public AudioClip explosionSound;

    private void Start()
    {
        playerHealth = FindFirstObjectByType<ArwingHealth>();   
    }

    void Update()
    {
        // Movimiento hacia adelante en el eje Z global
        transform.Translate(-Vector3.forward * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                Debug.Log("Player hit by enemy projectile");
                playerHealth.TakeDamage(damage);
            }
            if (impactEffect != null)
            {
                Vector3 collisionPoint = other.ClosestPoint(transform.position);
                Quaternion explosionRotation = Quaternion.LookRotation(collisionPoint - transform.position);
                GameObject explosion = Instantiate(impactEffect, collisionPoint, explosionRotation);

                // Destruir el efecto después de 1 segundo
                Destroy(explosion, 1f);
                AudioSource.PlayClipAtPoint(explosionSound, collisionPoint, 1f);
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag(ENEMY_TAG)) // Evitar colisión con otros proyectiles/enemigos
        {
            Destroy(gameObject);
        }
    }
}

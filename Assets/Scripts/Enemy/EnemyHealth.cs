using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [Header("Vida")]
    public int entityMaxHealth = 50;
    private int entityHealth;

    public GameObject impactEffect;
    public AudioClip damageSound;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityHealth = entityMaxHealth;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage, Vector3 impactPoint)
    {
	entityHealth -= damage;
        if (impactEffect != null)
        {
            Quaternion explosionRotation = Quaternion.LookRotation(impactPoint - transform.position);
            GameObject explosion = Instantiate(impactEffect, impactPoint, explosionRotation);

            Destroy(explosion, 1f);
        }

        // reproduce sound
        audioSource.PlayOneShot(damageSound);
        if (entityHealth <= 0) {
            Destroy(gameObject);
        }
	    
    }
}

using UnityEngine;

public class Explode : MonoBehaviour
{
    private const string ARWING_TAG = "Player";

    public GameObject explosionEffect;
    private AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	audioManager = FindFirstObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) // S'executa quan un altre objecte entra en el trigger
    {
	if(!other.CompareTag(ARWING_TAG))
	{
	    if (explosionEffect != null) Instantiate(explosionEffect, transform.position, transform.rotation);
	
            // Destrueix la bomba quan xoca amb qualsevol objecte
            Destroy(gameObject);
	    audioManager.PlayBoombSound();
	}
	
	/*if(effect != null)
	{
	    ParticleSystem particles = effect.GetComponent<ParticlesSystem>();
	    if(particles != null)
	    {
		particles.Stop();
		Destroy(effect, particles.main.duration);
	    }
	    else
	    {
		Destroy(effect);
	    }
	}*/
    }
}

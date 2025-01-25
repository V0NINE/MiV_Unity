using UnityEngine;

public class BombExplodeEffect : MonoBehaviour
{
    private const string ARWING_TAG = "Player";

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

            Vector3 bombPosition = transform.position;
            EffectsManager.Instance.PlayExplosion(bombPosition, true); // Explosión de bomba
            audioManager.PlayBoombSound();
            Destroy(gameObject);

        }
    }
}

using UnityEngine;

public class BombSpawner : MonoBehaviour
{

    public GameObject bomb_prefab;
    public Transform spawner;
    float spawn_force = 16f;

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

    public void SpawnBomb()
    {
	GameObject bomb = Instantiate(bomb_prefab, spawner.position, spawner.rotation);
	Rigidbody rb = bomb.GetComponent<Rigidbody>();

	if(rb != null) rb.AddForce(spawner.forward * spawn_force, ForceMode.Impulse);

        audioManager.PlayBombLaunchSound();

        Destroy(bomb, 3);
    }
}

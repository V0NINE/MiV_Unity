using UnityEngine;

public class BombSpawner : MonoBehaviour
{

    public GameObject bomb_prefab;
    public Transform spawner;
    float spawn_force = 16f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetMouseButtonDown(2)) SpawnBomb();    
    }

    void SpawnBomb()
    {
	GameObject bomb = Instantiate(bomb_prefab, spawner.position, spawner.rotation);
	Rigidbody rb = bomb.GetComponent<Rigidbody>();

	if(rb != null) rb.AddForce(spawner.forward * spawn_force, ForceMode.Impulse);

	Destroy(bomb, 3);
    }
}

using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet_prefab;
    public Transform left;
    public Transform right;
    Transform[] spawn;
    float spawn_force = 10f;
    float bullet_lifetime = 2f;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawn = new Transform[2] {left, right};
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) SpawnBullet();
    }

    void SpawnBullet() 
    {
	foreach (Transform spawn_point in spawn)
	{
		GameObject bullet = Instantiate(bullet_prefab, spawn_point.position, spawn_point.rotation);
		Rigidbody rb = bullet.GetComponent<Rigidbody>();

		if(rb != null) rb.AddForce(spawn_point.forward * spawn_force, ForceMode.Impulse);

		Destroy(bullet, bullet_lifetime);
	}
    }
}

using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet_prefab;
    public Transform left;
    public Transform right;
    float spawn_force = 10f;
    float bullet_lifetime = 2f;

    private AudioManager audioManager;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       audioManager = FindFirstObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
	{
	    SpawnBullet(left);
            EffectsManager.Instance.PlayPlayerShotEffect(left.position);
	}
	if(Input.GetMouseButtonDown(1))
	{
	    SpawnBullet(right);
            EffectsManager.Instance.PlayPlayerShotEffect(right.position);
        }
    }

    void SpawnBullet(Transform spawn_point) 
    {
	//foreach (Transform spawn_point in spawn)
	//{
	   Debug.Log("Rotacio del armwing: ");
		GameObject bullet = Instantiate(bullet_prefab, spawn_point.position, Quaternion.Euler(spawn_point.rotation.eulerAngles.x+90f, 
												      spawn_point.rotation.eulerAngles.y, 
												      0f));
		Rigidbody rb = bullet.GetComponent<Rigidbody>();

		if(rb != null) rb.AddForce(spawn_point.forward * spawn_force, ForceMode.Impulse);

		audioManager.PlayLaserSound();

		Destroy(bullet, bullet_lifetime);
	//}
    }
}

using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet_prefab;
    public Transform left;
    public Transform right;
    Transform[] spawn;
    float spawn_force = 10f;
    float bullet_lifetime = 2f;

    private AudioManager audioManager;
    private ShotEffect shotEffect;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawn = new Transform[2] {left, right};

       audioManager = FindFirstObjectByType<AudioManager>();
       shotEffect = FindFirstObjectByType<ShotEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
	{
	    SpawnBullet(left);
	    shotEffect.DoShotEffect(left);
	}
	if(Input.GetMouseButtonDown(1))
	{
	    SpawnBullet(right);
	    shotEffect.DoShotEffect(right);
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

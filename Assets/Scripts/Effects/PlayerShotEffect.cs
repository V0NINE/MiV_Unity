using UnityEngine;

public class PlayerShotEffect : MonoBehaviour
{

    public GameObject shotEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DoShotEffect(Transform spawn_point)
    {
  	GameObject effect = Instantiate(shotEffect, spawn_point.position, spawn_point.rotation);
	effect.transform.parent = spawn_point;

	ParticleSystem particles = effect.GetComponent<ParticleSystem>();
	if(particles != null)
	    Destroy(effect, particles.main.duration);
	else
	    Destroy(effect, 1f);
    }
}

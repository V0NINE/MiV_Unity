using UnityEngine;
using System.Collections.Generic;

public class BoostEffect : MonoBehaviour
{

    public Transform left;
    public Transform right;
    Transform[] spawn;

    public GameObject boostEffect;
    public GameObject smallBoostEffect;
    private List<GameObject> activeEffects = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawn = new Transform[2] {left, right};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBoostEffect()
    {
	if (boostEffect != null && smallBoostEffect != null)
	    foreach (Transform spawn_point in spawn)
	    {
		GameObject effect = Instantiate(boostEffect, spawn_point.position, spawn_point.rotation);
		effect.transform.parent = spawn_point;
		activeEffects.Add(effect);

		GameObject smallEffect = Instantiate(smallBoostEffect, spawn_point.position, spawn_point.rotation);
		smallEffect.transform.parent = spawn_point;
		activeEffects.Add(smallEffect);
 	    }
    }

    public void EndBoostEffect()
    {
	foreach (GameObject effect in activeEffects)
	{
	    if (effect != null)
	    {
		ParticleSystem particles = effect.GetComponent<ParticleSystem>();
		if (particles != null)
		{
		    particles.Stop();
		    Destroy(effect, particles.main.duration);
		}
		else Destroy(effect);
	    }
	}

	activeEffects.Clear();
    }
}

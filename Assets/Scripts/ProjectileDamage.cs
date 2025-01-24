using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{

    public int projectileDamage = 1;

    void OnTriggerEnter(Collider other)
    {
	if(other.CompareTag("Enemy"))
	{
	    EntityHealth entityHealth = other.GetComponent<EntityHealth>();

	    if(entityHealth != null)
	    {
		entityHealth.TakeDamage(projectileDamage);
		Destroy(gameObject);
	    }
	}
    }
}

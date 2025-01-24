using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{

    public int projectileDamage = 1;

    void OnTriggerEnter(Collider other)
    {
	if(other.CompareTag("Enemy"))
	{
	    EnemyHealth entityHealth = other.GetComponent<EnemyHealth>();

	    if(entityHealth != null)
	    {
                Vector3 impactPoint = other.ClosestPoint(transform.position);
                entityHealth.TakeDamage(projectileDamage, impactPoint);
		Destroy(gameObject);
	    }
	}
    }
}

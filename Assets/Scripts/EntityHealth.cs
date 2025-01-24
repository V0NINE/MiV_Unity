using UnityEngine;

public class EntityHealth : MonoBehaviour
{

    [Header("Vida")]
    public int entityMaxHealth = 50;
    private int entityHealth;    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityHealth = entityMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
	entityHealth -= damage;
	if(entityHealth <= 0)
	    Destroy(gameObject);
    }
}

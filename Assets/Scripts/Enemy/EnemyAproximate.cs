using UnityEngine;

public class EnemyAproximate : MonoBehaviour
{

    [SerializeField] float baseSpeed = 15f;
    [SerializeField] float initialZ = 68f;
    private float speedMultiplier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	transform.Translate(0,0,initialZ);
    }

    // Update is called once per frame
    void Update()
    {
	transform.Translate(0,0, -baseSpeed * speedMultiplier * Time.deltaTime);
	
	if(transform.position.z < -40)
	    transform.Translate(0,0,65);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
	speedMultiplier = multiplier;
    }
}

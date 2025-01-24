using UnityEngine;

public class BombsController : MonoBehaviour
{

    BombSpawner bombSpawner;

    public Animator animator;

    public int maxBombs = 1;
    private int amountOfBombs;
    public float bombCooldown = 3f;
    float cooldown = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    	bombSpawner = GetComponent<BombSpawner>();    
	amountOfBombs = maxBombs;
    }

    // Update is called once per frame
    void Update()
    {
	if(amountOfBombs > 0 && Input.GetMouseButtonDown(2))
	{
    	    bombSpawner.SpawnBomb();    
	    animator.SetTrigger("BombShot");
	    amountOfBombs--;
	}
	if(amountOfBombs < maxBombs)
	{
	    cooldown += Time.deltaTime;
	    if(cooldown >= bombCooldown)
	    {
		amountOfBombs++;
		cooldown = 0f;
	    }
	}
    }
}

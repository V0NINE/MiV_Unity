using UnityEngine;

public class BombsController : MonoBehaviour
{

    BombSpawner bombSpawner;

    public int amountOfBombs = 5;
    public float bombCooldown = 3f;
    float cooldown = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    	bombSpawner = GetComponent<BombSpawner>();    
    }

    // Update is called once per frame
    void Update()
    {
	if(amountOfBombs > 0 && Input.GetMouseButtonDown(2))
	{
	    Debug.Log("You should spawn some bombs");
    	    bombSpawner.SpawnBomb();    
	    amountOfBombs--;
	}
	if(amountOfBombs < 5)
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

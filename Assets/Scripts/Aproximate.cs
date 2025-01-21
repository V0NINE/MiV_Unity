using UnityEngine;

public class Aproximate : MonoBehaviour
{

    [SerializeField] float speed = 15f;
    [SerializeField] float initialZ = 68f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	transform.Translate(0,0,initialZ);
    }

    // Update is called once per frame
    void Update()
    {
	transform.Translate(0,0, -speed * Time.deltaTime);
	
	if(transform.position.z < -40)
	    transform.Translate(0,0,65);
    }
}

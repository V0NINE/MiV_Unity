using UnityEngine;

public class Aim : MonoBehaviour
{

    [SerializeField] float max_rotation = 3f;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouse_x = Input.mousePosition.x;
	float mouse_y = Input.mousePosition.y;

	Vector3 world = Camera.main.ScreenToWorldPoint(new Vector3(mouse_x, mouse_y, 10));


	Vector3 targetEulerAngles = rb.transform.localEulerAngles;
	rb.transform.localEulerAngles = new Vector3(Mathf.LerpAngle(targetEulerAngles.x, -Mathf.Clamp((world.y - rb.transform.position.y),-12,12) * max_rotation, .1f),
		       				    Mathf.LerpAngle(targetEulerAngles.y, Mathf.Clamp((world.x - rb.transform.position.x),-12,12) * max_rotation, .1f), 
						    targetEulerAngles.z);

	print(Mathf.Clamp((world.x - rb.transform.position.x), -12, 12));
    }

}

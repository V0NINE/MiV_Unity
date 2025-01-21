using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float leanLimit = 40f;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
	float vertical = Input.GetAxis("Vertical");
	
	rb.linearVelocity = new Vector3(horizontal, vertical, 0) * speed;

	if(horizontal != 0) HorizontalLean(horizontal);
	if(vertical != 0) VerticalLean(vertical);

	ClampPosition();
    }

    void ClampPosition()
    {
	Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);	    
	pos.x = Mathf.Clamp01(pos.x);
	pos.y = Mathf.Clamp01(pos.y);
	transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void HorizontalLean(float axis)
    {
	Vector3 targetEulerAngles = rb.transform.localEulerAngles;
	rb.transform.localEulerAngles = new Vector3(targetEulerAngles.x, targetEulerAngles.y, Mathf.LerpAngle(targetEulerAngles.z, -axis * leanLimit, .1f));
    }

    void VerticalLean(float axis)
    {
	Vector3 targetEulerAngles = rb.transform.localEulerAngles;
	rb.transform.localEulerAngles = new Vector3(Mathf.LerpAngle(targetEulerAngles.x, -axis * leanLimit, .1f), targetEulerAngles.y, targetEulerAngles.z);
    }
}

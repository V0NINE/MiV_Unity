using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 18f;
    [SerializeField] float leanLimit = 40f;

    public float lowerLimit = -8.8f;

    Rigidbody rb;

    private float oscillationTime = 0.0f;
    private bool isMoving = false;

    public float oscillationSpeed = 2.5f;   
    public float oscillationAmplitude = 0.002f; 


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

        if (horizontal != 0 || vertical != 0) isMoving = true;
        else isMoving = false;

        rb.linearVelocity = new Vector3(horizontal, vertical, 0) * speed;

	if(horizontal != 0) HorizontalLean(horizontal);
	if(vertical != 0) VerticalLean(vertical);

	ClampPosition();
        if (!isMoving)
        {
            Oscillate(transform);
        }
    }

    void Oscillate(Transform transform)
    {
        // Hacer oscilación con una función de seno
        oscillationTime += Time.deltaTime * oscillationSpeed;
        float oscillation = Mathf.Sin(oscillationTime) * oscillationAmplitude;

        // Aplicar la oscilación en el eje Y
        transform.position = new Vector3(transform.position.x, transform.position.y + oscillation, transform.position.z);
    }

    void ClampPosition()
    {
	Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);	    
	float yLowerLimit = Camera.main.WorldToViewportPoint(new Vector3(0, lowerLimit, 0)).y;
	pos.x = Mathf.Clamp01(pos.x);
	pos.y = Mathf.Clamp(pos.y, yLowerLimit, 1f);
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

using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    int speed = 4;

    [SerializeField]
    float rotationSpeed = 80f; // Velocitat de rotació en graus per segon

    [SerializeField] float maxRotationAngle = 20f;
    [SerializeField] float returnSpeed = 100f;

    Rigidbody rb;

    // Start és cridat una vegada abans d'executar Update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update és cridat una vegada per frame
    void Update()
    {
        // Obtenir els valors dels eixos
        float horizontal = Input.GetAxis("Horizontal"); // A i D (o esquerra i dreta)
        float vertical = Input.GetAxis("Vertical");     // W i S (o amunt i avall)

        // Crear el vector de moviment (només per desplaçament)
        Vector3 movement = new Vector3(horizontal, vertical, 0);

        // Si hi ha moviment vertical, mou l'objecte
        if (movement != Vector3.zero)
        {
            rb.linearVelocity = movement.normalized * speed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero; // Aturar l'objecte
        }

        // Gestió de la rotació
        float currentZRotation = transform.eulerAngles.z;
        if (horizontal != 0)
        {
            // Determinar la direcció de la rotació
            float rotationDirection = horizontal > 0 ? -1 : 1; // D gira horari, A gira antihorari
	    // Calcula la nova rotació
            currentZRotation = transform.eulerAngles.z;

            float newZRotation = currentZRotation + rotationDirection * rotationSpeed * Time.deltaTime;

            // Clampar la rotació al rang permès
            newZRotation = Mathf.Clamp(newZRotation, -maxRotationAngle, maxRotationAngle);

            // Aplicar la nova rotació
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
        }
	else 
	{
	     // Tornar gradualment a 0 graus
            float newZRotation = Mathf.MoveTowards(currentZRotation, 0, returnSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
	}
    }			   
}

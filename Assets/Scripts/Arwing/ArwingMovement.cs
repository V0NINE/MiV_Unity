using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float verticalSpeed = 80f;
    public float horizontalSpeed = 160f;
    public float maxRotationAngle = 25f; // Ángulo máximo de inclinación
    public float rotationSmoothing = 5f; // Suavizado de la rotación

    private float targetRotationX = 0f; // Rotación objetivo en X (inclinación)
    private float targetRotationY = 0f; // Rotación objetivo en Z (alabeo)

    public float limitX = 100.0f;
    public float limitY = 50.0f;

    private float oscillationTime = 0.0f;
    private bool isMoving = false;

    public float oscillationSpeed = 2.5f;    // Velocidad de la oscilación
    public float oscillationAmplitude = 0.05f; // Amplitud de la oscilación


    void Update()
    {
       
        // Movimiento vertical (adelante y atrás)
        float deltaY = Input.GetAxis("Vertical");
        Vector3 verticalMovement = Vector3.up * deltaY * verticalSpeed * Time.deltaTime;

        // Movimiento horizontal (izquierda y derecha)
        float deltaX = Input.GetAxis("Horizontal");
        Vector3 horizontalMovement = Vector3.right * deltaX * horizontalSpeed * Time.deltaTime;

        if (deltaX != 0 || deltaY != 0) isMoving = true;
        else isMoving = false;

        // Aplicar la traslación (solo la posición)
        transform.Translate(verticalMovement + horizontalMovement, Space.World);

        // Limitar la posición en X y Y
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -limitX, limitX);
        position.y = Mathf.Clamp(position.y, -limitY, limitY);
        transform.position = position;

        if (transform.position.x == limitX || transform.position.x == -limitX)
        {
            deltaX = 0;
        }

        if (transform.position.y == limitY || transform.position.y == -limitY)
        {
            deltaY = 0;
        }

        // Calcular rotaciones objetivo
        targetRotationX = -deltaX * maxRotationAngle; // Inclinar hacia adelante/atrás según el movimiento vertical
        targetRotationY = -deltaY * maxRotationAngle; // Inclinar hacia los lados según el movimiento horizontal

        // Suavizar la rotación actual hacia la rotación objetivo
        Quaternion targetRotation = Quaternion.Euler(targetRotationY, 0, targetRotationX);
        Quaternion targetRotation2 = Quaternion.Euler(0, targetRotationX, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * targetRotation2, rotationSmoothing * Time.deltaTime);

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
        transform.position = new Vector3(transform.position.x,  transform.position.y + oscillation, transform.position.z);
    }
}

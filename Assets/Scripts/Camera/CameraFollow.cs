using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float divisor = 1.5f; // Esto afecta solo a la posición

    void Update()
    {
        if (target != null)
        {
            // Mantener tu lógica actual para la posición
            Vector3 position = target.position;
            position.x = target.position.x / divisor;
            position.y = target.position.y / divisor;
            position.z = 0; // Asumimos que la cámara no tiene un offset en z respecto a target
            transform.position = position;

            // Corregir la lógica de rotación
            Quaternion targetRotation = target.rotation; // Obtener directamente la rotación de la nave
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f / divisor); // Suavizar la rotación
        }
    }
}

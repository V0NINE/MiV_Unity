using UnityEngine;

public class ProjectileRotation : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.linearVelocity != Vector3.zero)
        {
            // Rota la bala para que mire hacia su dirección de movimiento
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
            transform.Rotate(90, 0, 0);
        }
    }
}
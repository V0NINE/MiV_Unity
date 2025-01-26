using UnityEngine;

public class EnemyAproximate : MonoBehaviour
{
    [SerializeField] float baseSpeed = 15f;
    [SerializeField] float initialZ = 68f;
    [SerializeField] float resetZ = -20f;
    private float speedMultiplier = 1f;
    private Vector3 movementAxis = Vector3.back;

    void Start()
    {
        ResetPosition();
    }

    void Update()
    {
        MoveEnemy();
        CheckReset();
    }

    void MoveEnemy()
    {
        // Movimiento independiente de la rotación usando eje global
        transform.position += movementAxis * baseSpeed * speedMultiplier * Time.deltaTime;
    }

    void CheckReset()
    {
        if (transform.position.z <= resetZ)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        // Conserva la posición X actual para posibles patrones laterales
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            initialZ
        );
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}
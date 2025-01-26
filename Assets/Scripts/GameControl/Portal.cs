using UnityEngine;

public class Portal : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float respawnDistance = 100f;

    private Transform player;
    private bool moving = true;

    private LevelController levelController;

    public void Initialize(Transform playerTransform)
    {
        levelController = FindFirstObjectByType<LevelController>();
        player = playerTransform;
    }

    void Update()
    {
        if (moving)
        {
            // Movimiento hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Reposicionamiento si pasa al jugador
            if (Vector3.Dot(transform.position - player.position, player.forward) < -respawnDistance)
            {
                RepositionPortal();
            }
        }
    }

    void RepositionPortal()
    {
        transform.position = player.position + player.forward * 50f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moving = false;
            levelController.OnPortalEntered();
        }
    }
}
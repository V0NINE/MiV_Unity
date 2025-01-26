using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public float spawnAnimationDuration = 2f;  // Tiempo de la animación de caída
    public float spawnHeight = 20f;            // Altura inicial en Y
    public float fixedZPosition = 60f;         // Posición Z fija
    public float targetYPosition = -5f;        // Posición Y final

    void Start()
    {
        // Fijar posición Z y ajustar altura inicial
        Vector3 startPos = transform.position;
        startPos.y = spawnHeight;
        startPos.z = fixedZPosition;
        transform.position = startPos;

        StartCoroutine(SpawnAnimation());
    }

    IEnumerator SpawnAnimation()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(
            transform.position.x,  // Mantener X original
            targetYPosition,       // Y objetivo
            fixedZPosition         // Z fija
        );

        float elapsedTime = 0f;

        while (elapsedTime < spawnAnimationDuration)
        {
            // Interpolar solo la posición Y
            float newY = Mathf.Lerp(startPos.y, endPos.y, elapsedTime / spawnAnimationDuration);
            transform.position = new Vector3(
                transform.position.x,
                newY,
                fixedZPosition
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar posición final exacta
        transform.position = endPos;

        // (Opcional) Añadir aquí cualquier comportamiento posterior a la animación
    }
}
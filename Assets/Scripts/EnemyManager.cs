using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private float speedMultiplier = 1f;

    public void SetEnemySpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;

        // Actualitza tots els enemics només quan es canvia el multiplicador
        foreach (var entity in FindObjectsOfType<EntityAproximate>())
        {
            entity.SetSpeedMultiplier(speedMultiplier);
        }
    }
}

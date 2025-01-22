using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private float speedMultiplier = 1f;

    public void SetEntitySpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;

        foreach (var entity in FindObjectsByType<EntityAproximate>(FindObjectsSortMode.None))
        {
            entity.SetSpeedMultiplier(speedMultiplier);
        }
    }
}

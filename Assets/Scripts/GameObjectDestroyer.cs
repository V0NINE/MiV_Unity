using UnityEngine;

public class GameObjectDestroyer : MonoBehaviour
{
    void FixedUpdate() // Usamos FixedUpdate para sincronizar con la física
    {

        if (transform.position.z <= -20f) 
        {
            Destroy(gameObject);
        }
    }
}
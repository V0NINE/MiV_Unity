using UnityEngine;

public class LandTextureScroller : MonoBehaviour
{
    public float baseScrollSpeed = 1f; // Velocitat del moviment
    private float scrollSpeedMultiplier = 1f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calcula el desplaçament en funció del temps i la velocitat
        float offset = Time.time * baseScrollSpeed * scrollSpeedMultiplier;
        rend.material.SetTextureOffset("_BaseMap", new Vector2(0, offset));
    }

    public void SetScrollSpeedMultiplier(float multiplier)
    {
	scrollSpeedMultiplier = multiplier;
    }
}

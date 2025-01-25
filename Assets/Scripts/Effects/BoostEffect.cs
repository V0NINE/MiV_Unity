using UnityEngine;

public class BoostEffect : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBoostEffect()
    {
        EffectsManager.Instance.PlayBoostEffect(true);
    }

    public void EndBoostEffect()
    {
        EffectsManager.Instance.StopBoostEffects();
    }
}

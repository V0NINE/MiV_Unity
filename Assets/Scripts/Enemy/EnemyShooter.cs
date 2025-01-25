using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        StartCoroutine(ShootingRoutine());
    }

    System.Collections.IEnumerator ShootingRoutine()
    {
        while (true)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            audioManager.PlayEnemyShotSound();
            yield return new WaitForSeconds(fireRate);
        }
    }
}
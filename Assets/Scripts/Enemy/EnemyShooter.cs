using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    void Start()
    {
        StartCoroutine(ShootingRoutine());
    }

    System.Collections.IEnumerator ShootingRoutine()
    {
        while (true)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            yield return new WaitForSeconds(fireRate);
        }
    }
}
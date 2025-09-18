using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] Transform spawnPosition;

    public void SpawnBullet()
    {
        Instantiate(bulletPrefab, spawnPosition);
    }
}

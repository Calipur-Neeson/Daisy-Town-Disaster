using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    public int damage = 10;

    private SphereCollider sphereCollider;

    [SerializeField] private BulletSpawnChannelSO bulletSpawnChannelSO;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null )
        {
            Debug.LogWarning("We don't have sphere component!");
            sphereCollider = gameObject.AddComponent<SphereCollider>();
        }
        sphereCollider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
            bulletSpawnChannelSO?.RaiseEvent(new Context());
        }
    }

}

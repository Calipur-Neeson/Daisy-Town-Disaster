using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    public int damage = 10;

    private SphereCollider collider;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        if (collider == null )
        {
            Debug.LogWarning("We don't have sphere component!");
            collider = gameObject.AddComponent<SphereCollider>();
        }
        collider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);

    }
}

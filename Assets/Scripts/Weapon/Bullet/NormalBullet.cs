using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    private float speed;
    private float range;
    private int damage;

    private Vector3 startPosition;

    public void Init(float bulletSpeed, float bulletRange, int bulletDamage)
    {
        speed = bulletSpeed;
        range = bulletRange;
        damage = bulletDamage;

        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: 
        // if (other.CompareTag("Enemy")) other.GetComponent<Health>().TakeDamage(damage);

        Destroy(gameObject); 
    }
}

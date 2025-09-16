using UnityEngine;
using Unity.Netcode;

public class NormalBullet : NetworkBehaviour
{
    private float speed;
    private float range;
    private int damage;

    private Vector3 startPosition;

    private bool isActive;
    private WeaponController weaponController;
    public void Init(float bulletSpeed, float bulletRange, int bulletDamage)
    {
        speed = bulletSpeed;
        range = bulletRange;
        damage = bulletDamage;

        startPosition = transform.position;
        isActive = true;
    }

    private void Update()
    {
        if (!isActive) return;
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));

        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        // TODO: 
        // if (other.CompareTag("Enemy")) other.GetComponent<Health>().TakeDamage(damage);

        Deactivate(); 
    }
    
    private void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
        
        weaponController?.ReturnBullet(this.gameObject);
    }
    
    public void SetOwner(WeaponController owner)
    {
        weaponController = owner;
    }
}

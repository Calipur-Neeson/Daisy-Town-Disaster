using UnityEngine;
using Unity.Netcode;

public abstract class BaseBullet : NetworkBehaviour
{
    private float speed;
    private float range;
    private int damage;

    private Vector3 startPosition;

    private bool isActive;
    private WeaponController weaponController;

    protected abstract void Update();
    public virtual void Init(float bulletSpeed, float bulletRange, int bulletDamage)
    {
        speed = bulletSpeed;
        range = bulletRange;
        damage = bulletDamage;

        startPosition = transform.position;
        isActive = true;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        // TODO: 
        // if (other.CompareTag("Enemy")) other.GetComponent<Health>().TakeDamage(damage);

        Deactivate();
    }

    public virtual void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);

        weaponController?.ReturnBullet(this.gameObject);
    }

    public virtual void SetOwner(WeaponController owner)
    {
        weaponController = owner;
    }

    public bool GetIsActive() => isActive;
    public float GetSpeed() => speed;
    public float GetRange() => range;
    public Vector3 GetStartPosition() => startPosition;

}

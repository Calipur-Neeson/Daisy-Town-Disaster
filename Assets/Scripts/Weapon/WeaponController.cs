using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class WeaponController : NetworkBehaviour
{
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private Transform shootPoint; 

    private int currentAmmo;
    private bool isReloading = false;
    private bool canshoot = true;

    private float intervalTime;
    private float reloadTime;

    private void Awake()
    {
        InitWeapon();
        intervalTime = weaponData.shootInterval;
        reloadTime = weaponData.reloadTime;
    }

    private void Update()
    {
        if (!canshoot)
        {
            intervalTime -= Time.deltaTime;
            if (intervalTime <= 0)
            {
                canshoot = true;
                intervalTime = weaponData.shootInterval;
            }
        }

        if (isReloading)
        {
            reloadTime -= Time.deltaTime;
            if (reloadTime <= 0)
            {
                isReloading = false;
                reloadTime = weaponData.reloadTime;
                currentAmmo = weaponData.magazine;
                Debug.Log($"Done reloading! Current Ammo: {currentAmmo}");
            }
        }
    }
    private void InitWeapon()
    {
        if (!IsOwner || weaponData == null) return;

        currentAmmo = weaponData.magazine;
        //TODO: Equip weapon, move to the right position.
        Debug.Log($"Init Weapon: {weaponData.name}, Current Ammo: {currentAmmo}");
    }

    public void Shoot()
    {
        if (!IsOwner || isReloading) return;

        if (currentAmmo > 0)
        {
            Fire();
        }
        else
        {
            Reload();
        }
    }

    private void Fire()
    {
        if (canshoot && !isReloading)
        {
            canshoot = false;
            currentAmmo--;
            //TODO: Start shooting animation.

            FireOnServerRpc();
            GameObject bullet = Instantiate(weaponData.bulletPrefab, shootPoint);
            bullet.transform.SetParent(null);
            NormalBullet normalBullet = bullet.GetComponent<NormalBullet>();
            normalBullet.Init(weaponData.bulletSpeed, weaponData.range, weaponData.damage);
            Debug.Log($"{gameObject.name} Fire, Current Ammo: {currentAmmo}", transform);
        }
    }

    [ServerRpc]
    private void FireOnServerRpc()
    {
        FireOnClientRpc();
    }

    [ClientRpc]
    private void FireOnClientRpc()
    {
        if (!IsOwner)
        {
            GameObject bullet = Instantiate(weaponData.bulletPrefab, shootPoint);
            bullet.transform.SetParent(null);
            NormalBullet normalBullet = bullet.GetComponent<NormalBullet>();
            normalBullet.Init(weaponData.bulletSpeed, weaponData.range, weaponData.damage);
        }
    }

    public void Reload()
    {
        if (!IsOwner || isReloading || currentAmmo == weaponData.magazine) return;
        isReloading = true;
        //TODO: Start reload animation.
        Debug.Log("Reloading...");
    }
}

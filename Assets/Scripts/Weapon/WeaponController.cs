using UnityEngine;
using System.Collections.Generic;
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

    private List<GameObject> bulletPool = new List<GameObject>();
    private void Start()
    {
        InitWeapon();
        intervalTime = weaponData.shootInterval;
        reloadTime = weaponData.reloadTime;
        
        CreateBulletPool();
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
            int bulletIndex = GetInactiveBulletIndex();
            if (bulletIndex == -1) return;
            
            canshoot = false;
            currentAmmo--;
            //TODO: Start shooting animation.

            FireOnServerRpc(bulletIndex);
            
            Debug.Log($"{gameObject.name} Fire, Current Ammo: {currentAmmo}", transform);
        }
    }

    [ServerRpc]
    private void FireOnServerRpc(int bulletIndex)
    {
        ActivateBullet(bulletIndex);
        FireOnClientRpc(bulletIndex);
    }

    [ClientRpc]
    private void FireOnClientRpc(int bulletIndex)
    {
        ActivateBullet(bulletIndex);
    }

    public void Reload()
    {
        if (!IsOwner || isReloading || currentAmmo == weaponData.magazine) return;
        isReloading = true;
        //TODO: Start reload animation.
        Debug.Log("Reloading...");
    }
    
    private void CreateBulletPool()
    {
        for (int i = 0; i < weaponData.magazine; i++)
        {
            GameObject bullet = Instantiate(weaponData.bulletPrefab, shootPoint.position, Quaternion.identity,shootPoint);
            var nb = bullet.GetComponent<NormalBullet>();
            if (nb != null)
            {
                nb.SetOwner(this);
                bullet.SetActive(false);
                bulletPool.Add(bullet);
            }
        }
    }
    
    private int GetInactiveBulletIndex()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
                return i;
        }
        return -1;
    }
    
    private void ActivateBullet(int index)
    {
        if (index < 0 || index >= bulletPool.Count) return;

        GameObject bullet = bulletPool[index];
        bullet.transform.SetParent(null);
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;
        bullet.SetActive(true);

        var normalBullet = bullet.GetComponent<NormalBullet>();
        if (normalBullet != null)
        {
            normalBullet.Init(weaponData.bulletSpeed, weaponData.range, weaponData.damage);
        }
    }
    
    public void ReturnBullet(GameObject bullet)
    {
        if (!bulletPool.Contains(bullet))
        {
            Debug.LogWarning("Returned bullet does not belong to this pool.");
            return;
        }
        
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = Quaternion.identity;
        bullet.SetActive(false);
    }
}

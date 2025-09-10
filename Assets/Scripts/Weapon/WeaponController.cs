using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class WeaponController : NetworkBehaviour
{
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private Transform shootPoint; 

    private int currentAmmo;
    private bool isReloading = false;
    private Coroutine fireCoroutine;

    private void Start()
    {
        InitWeapon();
    }

    private void InitWeapon()
    {
        if (weaponData == null) return;

        currentAmmo = weaponData.magazine;
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
        currentAmmo--;
        GameObject bullet = Instantiate(weaponData.bulletPrefab, shootPoint);
        Debug.Log($"{gameObject.name} Fire, Current Ammo: {currentAmmo}", transform);
    }
    
    public void StartShooting() 
    {
        if (fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(AutoFire());
        }
    }

    public void StopShooting() 
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    private IEnumerator AutoFire()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(weaponData.shootInterval);
        }
    }

    
    #region Reloading
    public void Reload()
    {
        if (!IsOwner || isReloading) return;
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(weaponData.reloadTime);
        currentAmmo = weaponData.magazine;
        isReloading = false;
        Debug.Log($"Done! Current Ammo: {currentAmmo}");
    }
    #endregion

}

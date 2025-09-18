using UnityEngine;

public class WeaponPrefabbb : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform bulletPosition;

    public WeaponSO GetWeaponSO() => weaponSO;
    public Transform GetBulletPosition() => bulletPosition;
}

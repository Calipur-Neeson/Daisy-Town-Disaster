using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [Header("Set up")]
    public int damage;
    public int magazine;
    public float reloadTime;
    public float shootInterval;
    public float range;
    public float bulletSpeed;
    
    [Header("Prefab")]
    public WeaponType type;
    public GameObject weaponPrefab;
    public GameObject bulletPrefab;
}

public enum WeaponType
{
    Revolver,
    Rifle,
    Shotgun,
    Gatlinggun
}

using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public int damage;
    public int magazine;
    public float reloadTime;
    public float shootInterval;
    public float range;
    public float bulletSpeed;

    public WeaponType type;
    public GameObject weaponModle;
}

public enum WeaponType
{
    Revolver,
    Rifle,
    Shotgun,
    Gatlinggun
}

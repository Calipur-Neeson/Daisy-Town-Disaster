using UnityEngine;
using Unity.Netcode;

public class ChangePlayer : NetworkBehaviour
{
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private GameObject[] weaponPrefab;

    [SerializeField] protected WeaponController weaponController;

    private static int times = 0;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SetWeapon();
    }
    protected override void OnOwnershipChanged(ulong previous, ulong current)
    {
        SetWeapon();
    }

    public void SetWeapon()
    {
        UnityEngine.Random.InitState((int)OwnerClientId);
        foreach (GameObject go in weaponPosition)
        {
            Destroy(go);
        }
        Instantiate(weaponPrefab[times], weaponPosition);

    }
}

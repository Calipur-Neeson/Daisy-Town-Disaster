using UnityEngine;
using Unity.Netcode;

public class NormalBullet : BaseBullet
{
    protected override void Update()
    {
        if (!GetIsActive()) return;
        transform.Translate(Vector3.forward * (GetSpeed() * Time.deltaTime));

        if (Vector3.Distance(GetStartPosition(), transform.position) >= GetRange())
        {
            Deactivate();
        }
    }
}

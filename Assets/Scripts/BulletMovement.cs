using UnityEngine;
using static UnityEngine.GraphicsBuffer;


[RequireComponent(typeof(Rigidbody))]
public class BulletMovenment : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private Transform player;

    private Vector3 bulletToPlayer;
    private Quaternion targetRotation;

    private void Start()
    {
        var playerController = FindFirstObjectByType<PlayerController>();
        player = playerController.transform;
        
    }
    private void Update()
    {
        bulletToPlayer = transform.position - player.position;
        targetRotation = Quaternion.LookRotation(bulletToPlayer, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

        transform.position += -transform.forward * moveSpeed * Time.deltaTime;
        transform.position += Vector3.down * 0.5f * Time.deltaTime;
    }

}

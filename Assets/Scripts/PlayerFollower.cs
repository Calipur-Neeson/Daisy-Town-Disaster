using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform target;
    public float speed = 5.5f;
    public Vector3 offset = new Vector3(0, 5, -10);
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        _transform.position = Vector3.Lerp(_transform.position, desiredPosition, speed * Time.deltaTime);
        _transform.LookAt(target);
    }

}

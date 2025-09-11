using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = -20;

    private CharacterController controller;
    private InputActionMap inputMap;
    private InputAction _move;
    private Vector2 moveInput = Vector2.zero;
    private float yVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (inputActions == null) return;
        inputMap = inputActions.FindActionMap("Player");

        if (inputMap != null)
        {
            _move = inputMap.FindAction("Move");
        }
    }

    private void OnEnable()
    {
        if (_move != null)
        {
            _move.performed += OnPlayerMove;
            _move.canceled += OnPlayerStop;
        }
    }
    private void Update()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= moveSpeed;

        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }
        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;

        controller.Move(move * Time.deltaTime);
    }

    private void OnPlayerMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void OnPlayerStop(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
}

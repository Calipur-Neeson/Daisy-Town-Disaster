using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;

    [Header("Set up")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float gravity = -9.81f;

    private InputActionMap _actionMap;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lookAction;

    private CharacterController controller;
    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    
    private Vector3 velocity;
    private bool isGrounded;
    public bool isSprinting;
    private float xRotation = 0f;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _actionMap = actionAsset.FindActionMap("Player");

            if (_actionMap != null)
            {
                _moveAction = _actionMap.FindAction("Move");
                _jumpAction = _actionMap.FindAction("Jump");
                _lookAction = _actionMap.FindAction("Look");

                _actionMap.Enable();

                if (_moveAction != null)
                {
                    _moveAction.performed += OnPlayerMove;
                    _moveAction.canceled += OnPlayerStop;
                }
                if (_lookAction != null)
                {
                    _lookAction.performed += OnPlayerLook;
                    _lookAction.canceled += ctx => lookInput = Vector2.zero;
                }
                if (_jumpAction != null)
                {
                    _jumpAction.performed += ctx => Jump();
                }
            }
            else
            {
                Debug.LogWarning("InputActionMap 'Player' not found. Please check.");
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    private void OnDisable()
    {
        if (_moveAction != null)
        {
            _moveAction.performed -= OnPlayerMove;
            _moveAction.canceled -= OnPlayerStop;
        }
        if (_lookAction != null)
        {
            _lookAction.performed -= OnPlayerLook;
        }
    }
    
    private void Update()
    {
        if (!IsOwner) return;

        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);
        
        xRotation -= lookInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
    private void OnPlayerMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    private void OnPlayerStop(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
    
    private void OnPlayerLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}

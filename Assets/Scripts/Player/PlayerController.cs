using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;

    [Header("Set up")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float gravity = -9.81f;

    private InputActionMap _actionMap;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;
    private InputAction _shootAction;
    private InputAction _reloadAction;

    private CharacterController controller;
    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    
    private Vector3 velocity;
    private float speed;
    private bool isGrounded;
    private bool isSprinting;
    private bool isShoot;
    private float xRotation = 0f;
    private float sprintTime = 2f;
    private float sprintBarDrainSpeed = 3.0f;
    private float sprintBarRecoverSpeed = 0.5f;
    
    private WeaponController currentWeapon;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentWeapon = GetComponent<WeaponController>();
    }
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;
            Camera cam = GetComponentInChildren<Camera>();
            cam.enabled = false;
            return;
        }
        
        _actionMap = actionAsset.FindActionMap("Player");

        if (_actionMap != null)
        {
            _moveAction = _actionMap.FindAction("Move");
            _jumpAction = _actionMap.FindAction("Jump");
            _lookAction = _actionMap.FindAction("Look");
            _sprintAction = _actionMap.FindAction("Sprint");
            _shootAction = _actionMap.FindAction("Attack");
            _reloadAction = _actionMap.FindAction("Reload");

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
            if (_sprintAction != null)
            {
                _sprintAction.started += OnSprintStarted;
                _sprintAction.canceled += OnSprintCanceled;
            }
            if (_shootAction != null)
            {
                _shootAction.performed += ctx => StartShoot();
                _shootAction.canceled += ctx => StopShoot();
            }
            if (_reloadAction != null)
            {
                _reloadAction.performed += ctx => currentWeapon.Reload();
            }
        }
        else
        {
            Debug.LogWarning("InputActionMap 'Player' not found. Please check.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        RefillSprintEnergy();

        if (isShoot) 
            currentWeapon.Shoot();
    }

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        if (isSprinting && sprintTime > 0f)
        {
            speed = sprintSpeed;
        }
        else speed = moveSpeed;
        controller.Move(move * speed * Time.deltaTime);
        
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

    private void OnSprintStarted(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
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

    private void StartShoot()
    {
        isShoot = true;
    }

    private void StopShoot()
    {
        isShoot = false;
    }

    private void RefillSprintEnergy()
    {
        if (!isSprinting)
        {
            if (sprintTime >= 2.0f) return;
            sprintTime += Time.deltaTime * sprintBarRecoverSpeed;
        }

        else
        {
            if (sprintTime <= 0.0f) return;
            sprintTime -= Time.deltaTime * sprintBarDrainSpeed;
        }
    }
}

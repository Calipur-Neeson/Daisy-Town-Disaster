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

    private InputActionMap _actionMap;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lookAction;

    private CharacterController controller;
    public bool isSprinting;
    private Vector2 moveInput = Vector2.zero;
    private void OnEnable()
    {
        var playerMap = actionAsset.FindActionMap("Player");
        if (playerMap != null)
        {
            playerMap.Enable();
        }
        else
        {
            Debug.LogWarning("InputActionMap 'Player' not found. Please check.");
        }

        if (_moveAction != null)
        {
            _moveAction.performed += OnPlayerMove;
            _moveAction.canceled += OnPlayerStop;
        }
        else
        {
            Debug.LogWarning("Can not find Move action.");
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

    private void Jump()
    {

    }


}

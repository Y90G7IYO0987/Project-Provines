using UnityEngine;
using UnityEngine.InputSystem;

public struct PhysicsParameters
{
    public CharacterController CharacterController;
    public float MoveSpeed;
}

public class PlayerCharacteristic
{
    public PlayerCharacteristic()
    {

    }
}

public class PlayerPhysics : MonoBehaviour
{
    private Vector3 _moveDirection;
    private CharacterController _characterController;
    private float _movementSpeed;
    private float _verticalSpeed;
    private float _jumpHeight = 0.85f;

    private const float _gravity = -9.81f;
    private const float _groundGravity = -2f;

    public void Jump(InputAction.CallbackContext context)
    {
        if (!_characterController.isGrounded) return;

        _verticalSpeed = Mathf.Sqrt(-2f * _gravity * _jumpHeight);
    }

    private void Update()
    {
        Vector3 horizontalSpeed = (transform.right * _moveDirection.x + transform.forward * _moveDirection.z) * _movementSpeed;
        
        if (_characterController.isGrounded && _verticalSpeed < 0)
        {
            _verticalSpeed = _groundGravity;
        }
        _verticalSpeed += _gravity * Time.deltaTime;

        Vector3 moveVector = horizontalSpeed;
        moveVector.y = _verticalSpeed;

        _characterController.Move(moveVector * Time.deltaTime);
    }

    public void SetMoveDirection(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        _moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
    }

    public void Initialize(PhysicsParameters physicsParameters)
    {
        _movementSpeed = physicsParameters.MoveSpeed;
        _characterController = physicsParameters.CharacterController;
    }
}

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private PlayerData playerData;

    private InputActions _inputActions;
    private PlayerPhysics _playerPhysics;
    private PhysicsParameters _physicsParameters;
    private CharacterController _characterController;

    private void Awake()
    {
        _playerPhysics = gameObject.AddComponent<PlayerPhysics>();
        _characterController = GetComponent<CharacterController>();

        _physicsParameters = new PhysicsParameters
        {
            CharacterController = _characterController,
            MoveSpeed = _moveSpeed
        };

        playerData.Prefab = gameObject;
    }

    private void Start()
    {
        _inputActions = _gameInput.InputActions;

        _playerPhysics.Initialize(_physicsParameters);

        _inputActions.Player.Move.performed += _playerPhysics.SetMoveDirection;
        _inputActions.Player.Move.canceled += _playerPhysics.SetMoveDirection;
        _inputActions.Player.Jump.performed += _playerPhysics.Jump;
    }
}

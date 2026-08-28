using UnityEngine;
using UnityEngine.InputSystem;

public struct PhysicsParameters
{
    public CharacterController CharacterController;
    public float MoveSpeed;
}

public class PlayerPhysics : MonoBehaviour
{
    public Vector3 MoveDirection { get; private set; }
    private CharacterController _characterController;
    private float _movementSpeed;
    private float _verticalSpeed;
    private float _jumpHeight = 1.1f;
    private float _runMultiplier = 1f;
    private bool _isRunning;
    private bool _isGrounded;

    private const float _gravity = -9.81f;
    private const float _groundGravity = -2f;

    public bool IsPlayerRunning() => _isRunning;
    public bool IsGroundedPlayer() => _isGrounded;

    private void Update()
    {
        _isGrounded = _characterController.isGrounded;
        Debug.Log($"Is grounded - {_isGrounded}.");

        Vector3 horizontalSpeed = (transform.right * MoveDirection.x + transform.forward * MoveDirection.z) * (_movementSpeed * _runMultiplier);

        if (_characterController.isGrounded && _verticalSpeed < 0)
        {
            _verticalSpeed = _groundGravity;
        }
        _verticalSpeed += _gravity * Time.deltaTime;

        Vector3 moveVector = horizontalSpeed;
        moveVector.y = _verticalSpeed;

        _characterController.Move(moveVector * Time.deltaTime);
    }

    public void Initialize(PhysicsParameters physicsParameters)
    {
        _movementSpeed = physicsParameters.MoveSpeed;
        _characterController = physicsParameters.CharacterController;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!_characterController.isGrounded) return;

        _verticalSpeed = Mathf.Sqrt(-2f * _gravity * _jumpHeight);
    }

    public void Run(InputAction.CallbackContext context)
    {
        _isRunning = true;
        _runMultiplier /= 0.5f;
    }

    public void StopRunning(InputAction.CallbackContext context)
    {
        _isRunning = false;
        _runMultiplier = 1f;
    }

    public void SetMoveDirection(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        MoveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
    }    
}

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject mainCamera;

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

        GameObject camera = Instantiate(mainCamera, transform.parent);
        var cameraMovement = camera.GetComponent<CameraMovement>();
        cameraMovement.SetPlayerData(playerData);

        _inputActions.Player.Move.performed += _playerPhysics.SetMoveDirection;
        _inputActions.Player.Move.canceled += _playerPhysics.SetMoveDirection;
        _inputActions.Player.Jump.performed += _playerPhysics.Jump;

        _inputActions.Player.Run.performed += _playerPhysics.Run;
        _inputActions.Player.Run.canceled += _playerPhysics.StopRunning;
    }

    private void OnDestroy()
    {
        _inputActions.Player.Move.performed -= _playerPhysics.SetMoveDirection;
        _inputActions.Player.Move.canceled -= _playerPhysics.SetMoveDirection;
        _inputActions.Player.Jump.performed -= _playerPhysics.Jump;

        _inputActions.Player.Run.performed -= _playerPhysics.Run;
        _inputActions.Player.Run.canceled -= _playerPhysics.StopRunning;
    }
}

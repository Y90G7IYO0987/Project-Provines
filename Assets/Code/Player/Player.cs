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
    public Vector3 HorizontalSpeed { get; private set; }

    public float RequireMoveLength = 0.15f;

    private CharacterController _characterController;
    private float _movementSpeed;
    private float _verticalSpeed;
    private float _jumpHeight = 1.1f;
    private float _runMultiplier = 1f;
    private bool _isRunning;
    private bool _isGrounded;
    private PlayerVisual _playerVisual;
    private float _rotationSpeed = 10.0f;

    private const float _gravity = -9.81f;
    private const float _groundGravity = -2f;

    public bool IsPlayerRunning() => _isRunning;
    public bool IsGroundedPlayer() => _isGrounded;

    private void Awake()
    {
        _playerVisual = GetComponent<PlayerVisual>();
    }

    private void Update()
    {
        _isGrounded = _characterController.isGrounded;

        HorizontalSpeed = (transform.right * MoveDirection.x + transform.forward * MoveDirection.z) * (_movementSpeed * _runMultiplier);

        if (HorizontalSpeed.x > 0f)
        {
            float horizontalAxis = Input.GetAxis("Horizontal");
            Vector3 targetDirection = Vector3.right * Mathf.Sign(horizontalAxis);

            Debug.Log($"Direction - {targetDirection}");

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (_rotationSpeed * Time.deltaTime));
        }

        if (_characterController.isGrounded && _verticalSpeed < 0)
        {
            _verticalSpeed = _groundGravity;
        }

        _verticalSpeed += _gravity * Time.deltaTime;

        Vector3 finalVector = HorizontalSpeed;
        finalVector.y = _verticalSpeed;

        _characterController.Move(finalVector * Time.deltaTime);
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
        if (HorizontalSpeed.magnitude < RequireMoveLength) return;
        if (_playerVisual.CurrentStamina == 0f) return;

        _isRunning = true;
        _runMultiplier = 2f;
        _playerVisual.SetRunning(true);
    }

    public void StopRunning(InputAction.CallbackContext context)
    {
        _isRunning = false;
        _runMultiplier = 1f;
        _playerVisual.SetRunning(false);
    }

    public void StoppingRun()
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
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private PlayerData playerData;

    private InputActions _inputActions;
    private PlayerPhysics _playerPhysics;
    private PhysicsParameters _physicsParameters;
    private CharacterController _characterController;

    public void SetInputActions(InputActions inputActions) => _inputActions = inputActions;

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
        _playerPhysics.Initialize(_physicsParameters);

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

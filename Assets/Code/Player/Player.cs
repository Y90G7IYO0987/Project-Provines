using UnityEngine;
using UnityEngine.InputSystem;

public struct PhysicsParameters
{
    public CharacterController CharacterController;
    public float MoveSpeed;
}

public class PlayerPhysics : MonoBehaviour
{
    private Vector3 _moveDirection;
    private float _movementSpeed;
    private CharacterController _characterController;
    private float _verticalVelocity;
    private float _gravity = -9.81f;

    private void Update()
    {
        _verticalVelocity += _gravity * Time.deltaTime;

        if (_characterController.isGrounded && _verticalVelocity < 0) _verticalVelocity = -2f;

        Vector3 movePosition = (transform.right * _moveDirection.x + transform.forward * _moveDirection.z).normalized;
        Vector3 finalPosition = movePosition * (Time.deltaTime * _movementSpeed);

        finalPosition.y = _verticalVelocity * Time.deltaTime;

        _characterController.Move(finalPosition);
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
    }

    private void Start()
    {
        _inputActions = _gameInput.InputActions;

        _inputActions.Player.Move.performed += _playerPhysics.SetMoveDirection;
        _inputActions.Player.Move.canceled += _playerPhysics.SetMoveDirection;
        _playerPhysics.Initialize(_physicsParameters);
    }
}

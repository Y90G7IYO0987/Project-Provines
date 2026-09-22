using ProjectProvines.Core.Player;
using ProjectProvines.Core.Utils;
using ProjectProvines.Unity.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectProvines.Unity.Player
{
    /// <summary>
    /// Описывает физику игрока.
    /// </summary>
    public class Physics : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [Space]
        [SerializeField] private float moveSpeed = 4.0f;
        [SerializeField] private float jumpHeight = 3.0f;

        private const float Gravity = -9.81f;
        private const float GroundedGravity = -2.0f;

        private PlayerCore _playerCore;
        private Orientation _orientation;
        private PlayerInputController _playerInput;
        private PlayerUtils _playerUtils;

        private Transform _targetCamera;
        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _orientation = GetComponent<Orientation>();

            _playerCore = new PlayerCore(moveSpeed, jumpHeight, Gravity, GroundedGravity);
        }

        private void Start()
        {
            _playerUtils = playerData.PlayerUtils;
            _playerInput = _playerUtils.GetPlayerInput();
            _targetCamera = _playerUtils.GetCameraController().transform;

            _playerInput.OnRun += OnPlayerRun;
            _playerInput.OnJump += OnPlayerJump;
        }

        private void Update()
        {
            Vector2 input = _playerInput.MoveInputValue;
            Vector3 targetDirection = GetWorldDirection(input);
            Debug.Log($"dir: {targetDirection}");
            if (targetDirection.sqrMagnitude > 0.01f)
            {
                _orientation.RotateTowards(targetDirection);
            }
            _playerCore.SetMoveDirection(targetDirection);

            _playerCore.SetPlayerGrounded(_characterController.isGrounded);

            Vector3 horizontalVelocity = _playerCore.CalculateHorizontalVelocity();
            float verticalVelocity = _playerCore.CalculateVerticalVelocity(Time.deltaTime);
            Vector3 moveVector = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

            _characterController.Move(moveVector * Time.deltaTime);
        }

        /// <summary>
        /// Считает направление движения игрока
        /// </summary>
        /// <param name="context">Параметр для считывания вектора</param>
        /// <returns>Направление движения игрока</returns>
        private Vector2 GetInputVector(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            return inputVector;
        }

        private Vector3 GetWorldDirection(Vector2 inputVector)
        {
            Vector3 forward = _targetCamera.forward;
            Vector3 right = _targetCamera.right;
            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 direction = forward * inputVector.y + right * inputVector.x;
            if (direction.sqrMagnitude > 1f)
                direction.Normalize();

            return direction;
        }       

        private void OnPlayerRun(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                _playerCore.StartRunning();
            else if (context.phase == InputActionPhase.Canceled)
                _playerCore.StopRunning();
        }

        private void OnPlayerJump()
        {
            _playerCore.TryJump();
        }

        private void OnDestroy()
        {
            _playerInput.OnRun -= OnPlayerRun;
            _playerInput.OnJump -= OnPlayerJump;
        }
    }
}
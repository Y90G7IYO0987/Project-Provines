using ProjectProvines.Core.Player;
using ProjectProvines.Core.Utils;
using ProjectProvines.Unity.Animations;
using ProjectProvines.Unity.Input;
using ProjectProvines.Unity.Scriptables;
using ProjectProvines.Unity.UI;
using ProjectProvines.Unity.Weapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectProvines.Unity.Player
{
    /// <summary>
    /// Describes the player’s physics.
    /// </summary>
    [RequireComponent(typeof(PlayerData))]
    public class PlayerPhysics : MonoBehaviour
    {
        public float XAxis { get; private set; }
        public float ZAxis { get; private set; }

        public bool IsRunning { get; private set; }

        public WeaponStaticBox WeaponStaticBox { get; private set; }

        [SerializeField] private PlayerData playerData;

        [SerializeField] private float moveSpeed = 4.0f;
        [SerializeField] private float jumpHeight = 3.0f;

        private PlayerCore _playerCore;
        private Orientation _orientation;
        private PlayerInputController _playerInput;
        private PlayerUtils _playerUtils;
        private PlayerAnimations _playerAnimations;
        private BarSliders _barSliders;

        private Transform _targetCamera;
        private CharacterController _characterController;

        private const float Gravity = -9.81f;
        private const float GroundedGravity = -2.0f;

        public void SetBarSliders(BarSliders barSliders) => _barSliders = barSliders;
        public void SetWeaponStaticBox(WeaponStaticBox staticBox) => WeaponStaticBox = staticBox;

        /// <summary>
        /// Initialization of the motion controller and the necessary motion classes.
        /// </summary>
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _orientation = GetComponent<Orientation>();
            _playerAnimations = GetComponent<PlayerAnimations>();

            _playerCore = new PlayerCore(moveSpeed, jumpHeight, Gravity, GroundedGravity);
        }

        /// <summary>
        /// Initializing utilities via SO, subscribing to events.
        /// </summary>
        private void Start()
        {
            _playerUtils = playerData.PlayerUtils;
            _playerInput = _playerUtils.GetPlayerInput();
            _targetCamera = _playerUtils.GetCameraController().transform;

            _playerInput.OnRun += OnPlayerRun;
            _playerInput.OnJump += OnPlayerJump;
        }

        /// <summary>
        /// It creates movement for the player and ensures the necessary rotation.
        /// </summary>
        private void Update()
        {
            Vector2 input = _playerInput.MoveInputValue;
            Vector3 targetDirection = GetWorldDirection(input);
            if (targetDirection.sqrMagnitude > 0.01f)
            {
                _orientation.RotateTowards(targetDirection);
            }
            _playerCore.SetMoveDirection(targetDirection);

            UpdateAnimationAxes(targetDirection);

            _playerCore.SetPlayerGrounded(_characterController.isGrounded);

            Vector3 horizontalVelocity = _playerCore.CalculateHorizontalVelocity();
            float verticalVelocity = _playerCore.CalculateVerticalVelocity(Time.deltaTime);
            Vector3 moveVector = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

            _characterController.Move(moveVector * Time.deltaTime);

            IsRunning = _playerCore.IsPlayerRunning();
        }

        /// <summary>
        /// Calculates the displacement vector relative to the camera’s position.
        /// </summary>
        /// <param name="inputVector">Initial input vector.</param>
        /// <returns>The character’s movement vector.</returns>
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

        /// <summary>
        /// It transforms the global trend into a local one.
        /// </summary>
        /// <param name="worldDirection">Global direction.</param>
        private void UpdateAnimationAxes(Vector3 worldDirection)
        {
            if (worldDirection.sqrMagnitude < 0.01f)
            {
                XAxis = 0;
                ZAxis = 0;
                return;
            }

            Vector3 localDirection = transform.InverseTransformDirection(worldDirection);

            XAxis = localDirection.x;
            ZAxis = localDirection.z;
        }

        /// <summary>
        /// Processes the player’s running.
        /// </summary>
        private void OnPlayerRun(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                _playerCore.StartRunning();
                _barSliders.StaminaModifier();
            }
            else if (context.phase == InputActionPhase.Canceled)
                _playerCore.StopRunning();
        }

        /// <summary>
        /// Processes the player’s jump.
        /// </summary>
        private void OnPlayerJump()
        {
            if (_characterController.isGrounded)
            {
                _playerCore.TryJump();
                _playerAnimations.TryJump();
            }
        }

        /// <summary>
        /// Disconnection from events.
        /// </summary>
        private void OnDestroy()
        {
            _playerInput.OnRun -= OnPlayerRun;
            _playerInput.OnJump -= OnPlayerJump;
        }
    }
}
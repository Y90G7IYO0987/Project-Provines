using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectProvines.Unity.Input
{
    /// <summary>
    /// The main class for processing player input.
    /// </summary>
    [RequireComponent(typeof(UnityInputControls))]
    public class PlayerInputController : MonoBehaviour
    {
        public event Action<InputAction.CallbackContext> OnRun;
        public event Action OnJump;
        public event Action OnAttackInput;

        /// <summary>
        /// A parameter containing the player’s movement vector.
        /// </summary>
        public Vector2 MoveInputValue => _playerControls.Player.Move.ReadValue<Vector2>();

        [SerializeField] private UnityInputControls _inputControls;

        private PlayerControls _playerControls;

        /// <summary>
        /// Initializes the input class.
        /// Subscribes to the main input events.
        /// </summary>
        private void Start()
        {
            _playerControls = _inputControls.GetPlayerControls();

            _playerControls.Player.Run.started += OnPlayerRun;
            _playerControls.Player.Run.canceled += OnPlayerRun;
            _playerControls.Player.Jump.performed += OnPlayerJump;
            _playerControls.Player.Attack.performed += OnPlayerAttack;
        }

        private void OnPlayerRun(InputAction.CallbackContext context)
        {
            OnRun?.Invoke(context);
        }

        private void OnPlayerJump(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }

        private void OnPlayerAttack(InputAction.CallbackContext context)
        {
            OnAttackInput?.Invoke();
        }

        /// <summary>
        /// Unsubscribe from input events.
        /// </summary>
        private void OnDestroy()
        {
            _playerControls.Player.Run.started -= OnPlayerRun;
            _playerControls.Player.Run.canceled -= OnPlayerRun;
            _playerControls.Player.Jump.performed -= OnPlayerJump;
            _playerControls.Player.Attack.performed -= OnPlayerAttack;
        }
    }
}
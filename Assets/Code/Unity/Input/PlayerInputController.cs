using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectProvines.Unity.Input
{
    public class PlayerInputController : MonoBehaviour
    {
        public event Action<InputAction.CallbackContext> OnRun;
        public event Action OnJump;

        public Vector2 MoveInputValue => _playerControls.Player.Move.ReadValue<Vector2>();

        [SerializeField] private UnityInputControls _inputControls;

        private PlayerControls _playerControls;

        private void Start()
        {
            _playerControls = _inputControls.GetPlayerControls();

            _playerControls.Player.Run.started += OnPlayerRun;
            _playerControls.Player.Run.canceled += OnPlayerRun;
            _playerControls.Player.Jump.performed += OnPlayerJump;
        }

        private void OnPlayerRun(InputAction.CallbackContext context)
        {
            OnRun?.Invoke(context);
        }

        private void OnPlayerJump(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }

        private void OnDestroy()
        {
            _playerControls.Player.Run.started -= OnPlayerRun;
            _playerControls.Player.Run.canceled -= OnPlayerRun;
            _playerControls.Player.Jump.performed -= OnPlayerJump;
        }
    }
}
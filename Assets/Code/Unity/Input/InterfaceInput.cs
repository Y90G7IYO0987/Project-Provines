using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectProvines.Unity.Input
{
    public class InterfaceInput : MonoBehaviour
    {
        /// <summary>
        /// Передает (-1,1) в зависимости от нажатой кнопки.
        /// </summary>
        public event Action<int> OnMenuSwitchers;

        public event Action OnCellEquipping;
        public event Action OnCellStopHolding;

        [SerializeField] private UnityInputControls inputControls;

        private PlayerControls _playerControls;

        private void Start()
        {
            _playerControls = inputControls.GetPlayerControls();

            _playerControls.UI.MenuSwitch.performed += OnMenuSwitch;

            _playerControls.UI.ItemEquipping.performed += OnCellInput;
            _playerControls.UI.StopHoldingCell.performed += OnStopCellInput;
        }

        /// <summary>
        /// Обрабатывает нажатия и посылает информацию через события.
        /// </summary>
        /// <param name="context">Параметры нажатия.</param>
        private void OnMenuSwitch(InputAction.CallbackContext context)
        {
            OnMenuSwitchers?.Invoke(context.control.name == "rightArrow" ? 1 : -1);
        }

        private void OnCellInput(InputAction.CallbackContext context)
        {
            OnCellEquipping?.Invoke();
        }

        private void OnStopCellInput(InputAction.CallbackContext context)
        {
            OnCellStopHolding?.Invoke();
        }

        private void OnDestroy()
        {
            _playerControls.UI.MenuSwitch.performed -= OnMenuSwitch;
            _playerControls.UI.ItemEquipping.performed -= OnCellInput;
            _playerControls.UI.StopHoldingCell.performed -= OnStopCellInput;
        }
    }
}
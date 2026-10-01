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

        [SerializeField] private UnityInputControls inputControls;

        private PlayerControls _playerControls;

        private void Start()
        {
            _playerControls = inputControls.GetPlayerControls();

            _playerControls.UI.MenuSwitch.performed += OnMenuSwitch;
        }

        /// <summary>
        /// Обрабатывает нажатия и посылает информацию через события.
        /// </summary>
        /// <param name="context">Параметры нажатия.</param>
        private void OnMenuSwitch(InputAction.CallbackContext context)
        {
            OnMenuSwitchers?.Invoke(context.control.name == "rightArrow" ? 1 : -1);
        }

        private void OnDestroy()
        {
            _playerControls.UI.MenuSwitch.performed -= OnMenuSwitch;
        }
    }
}
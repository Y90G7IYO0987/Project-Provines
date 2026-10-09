using ProjectProvines.Core.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectProvines.Unity.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private GameObject menuView;

        private PlayerUtils _playerUtils;
        private PlayerControls _playerControls;

        private void Start()
        {
            _playerUtils = playerData.PlayerUtils;
            _playerControls = _playerUtils.GetInputControls().GetPlayerControls();

            _playerControls.UI.OpenMenu.performed += OpenMenu;
        }

        private void OpenMenu(InputAction.CallbackContext context)
        {
            menuView.SetActive(!menuView.activeInHierarchy);
        }
    }
}

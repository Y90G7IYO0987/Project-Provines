using ProjectProvines.Core.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

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
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}

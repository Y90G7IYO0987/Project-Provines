using ProjectProvines.Core.Player;
using ProjectProvines.Core.Utils;
using ProjectProvines.Unity.Camera;
using ProjectProvines.Unity.Input;
using ProjectProvines.Unity.Scriptables;
using ProjectProvines.Unity.UI;
using ProjectProvines.Unity.Weapon;
using UnityEngine;

namespace ProjectProvines.Unity.Player
{
    /// <summary>
    /// Creates a player in the game.
    /// Initializes the necessary player states.
    /// Connects the necessary player events.
    /// </summary>
    [RequireComponent(typeof(GameObject))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerData))]

    [RequireComponent(typeof(CameraController))]
    [RequireComponent(typeof(UnityInputControls))]
    [RequireComponent(typeof(PlayerSliders))]

    [RequireComponent(typeof(WeaponController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private PlayerInputController playerInput;
        [SerializeField] private PlayerData playerData;

        [SerializeField] private CameraController cameraController;
        [SerializeField] private UnityInputControls inputControls;
        [SerializeField] private PlayerSliders playerSliders;

        [SerializeField] private WeaponController weaponController;

        private PlayerUtils _playerUtils;

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the player, their state, and passes the necessary data.
        /// </summary>
        private void Initialize()
        {
            var player = Instantiate(playerPrefab, transform.parent);
            playerData.Prefab = player;
            _playerUtils = new PlayerUtils(playerInput, cameraController, inputControls);
            playerData.PlayerUtils = _playerUtils;

            var physics = playerData.Prefab.GetComponent<PlayerPhysics>();
            physics.SetPlayerSliders(playerSliders);

            playerData.WeaponController = weaponController;
        }
    }
}

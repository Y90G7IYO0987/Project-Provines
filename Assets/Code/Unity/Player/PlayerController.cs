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
    [RequireComponent(typeof(BarSliders))]

    [RequireComponent(typeof(WeaponController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private PlayerInputController playerInput;
        [SerializeField] private PlayerData playerData;

        [SerializeField] private CameraController cameraController;
        [SerializeField] private UnityInputControls inputControls;
        [SerializeField] private BarSliders barSliders;

        [SerializeField] private WeaponController weaponController;

        private PlayerUtils _playerUtils;
        private PlayerProperties _playerProperties;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {            
            _playerProperties.OnPlayerDeath += OnPlayerDeath;
        }

        /// <summary>
        /// Initializes the player, their state, and passes the necessary data.
        /// </summary>
        private void Initialize()
        {
            _playerProperties = new PlayerProperties(playerData.MaxHealth, playerData.MaxMagic, playerData.MaxStamina);

            var player = Instantiate(playerPrefab, transform.parent);
            playerData.Prefab = player;
            _playerUtils = new PlayerUtils(playerInput, cameraController, inputControls, _playerProperties);
            playerData.PlayerUtils = _playerUtils;

            var physics = playerData.Prefab.GetComponent<PlayerPhysics>();
            physics.SetBarSliders(barSliders);

            playerData.WeaponController = weaponController;
        }

        /// <summary>
        /// Player death handler.
        /// </summary>
        private void OnPlayerDeath()
        {
            Debug.Log("The player is die!");
        }

        private void OnDestroy()
        {
            _playerProperties.OnPlayerDeath -= OnPlayerDeath;
        }
    }
}

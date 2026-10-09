using ProjectProvines.Core.Player;
using ProjectProvines.Core.Utils;
using ProjectProvines.Unity.Camera;
using ProjectProvines.Unity.Input;
using UnityEngine;

namespace ProjectProvines.Unity.Player
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private PlayerInputController playerInput;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private UnityInputControls inputControls;

        private PlayerUtils _playerUtils;
        private PlayerProperties _playerProperties;

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Создает игрока в игре.
        /// </summary>
        private void Initialize()
        {
            _playerProperties = new PlayerProperties(playerData.MaxHealth, playerData.MaxMagic, playerData.MaxStamina);

            var player = Instantiate(playerPrefab, transform.parent);
            playerData.Prefab = player;
            _playerUtils = new PlayerUtils(playerInput, cameraController, inputControls, _playerProperties);
            playerData.PlayerUtils = _playerUtils;
        }
    }
}

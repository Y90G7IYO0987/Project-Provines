using ProjectProvines.Core.Utils;
using ProjectProvines.Unity.Input;
using UnityEngine;

namespace ProjectProvines.Unity.Player
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerInputController playerInput;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private GameObject playerPrefab;

        private PlayerUtils _playerUtils;

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Создает игрока в игре.
        /// </summary>
        private void Initialize()
        {
            var player = Instantiate(playerPrefab, transform.parent);
            playerData.Prefab = player;
            _playerUtils = new PlayerUtils(playerInput, cameraController);
            playerData.PlayerUtils = _playerUtils;
        }
    }
}

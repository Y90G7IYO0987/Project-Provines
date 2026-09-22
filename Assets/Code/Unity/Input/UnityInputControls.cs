using UnityEngine;

namespace ProjectProvines.Unity.Input
{
    public class UnityInputControls : MonoBehaviour
    {
        private PlayerControls _playerControls;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }

        /// <returns>События действий игрока</returns>
        public PlayerControls GetPlayerControls()
        {
            return _playerControls;
        }
    }
}
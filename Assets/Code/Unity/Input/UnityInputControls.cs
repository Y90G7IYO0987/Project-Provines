using UnityEngine;

namespace ProjectProvines.Unity.Input
{
    /// <summary>
    /// The main class with an input instance.
    /// </summary>
    public class UnityInputControls : MonoBehaviour
    {
        private PlayerControls _playerControls;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }

        /// <returns>Instance of input parameters.</returns>
        public PlayerControls GetPlayerControls()
        {
            return _playerControls;
        }
    }
}
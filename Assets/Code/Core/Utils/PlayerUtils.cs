using ProjectProvines.Core.Player;
using ProjectProvines.Unity.Camera;
using ProjectProvines.Unity.Input;

namespace ProjectProvines.Core.Utils
{
    public class PlayerUtils
    {
        private readonly PlayerInputController _playerInput;
        private readonly CameraController _cameraController;
        private readonly UnityInputControls _inputControls;
        private readonly PlayerProperties _playerProperties;

        public PlayerUtils(PlayerInputController input, CameraController camera,
            UnityInputControls inputControls, PlayerProperties properties)
        {
            _playerInput = input;
            _cameraController = camera;
            _inputControls = inputControls;
            _playerProperties = properties;
        }

        public PlayerInputController GetPlayerInput() => _playerInput;
        public CameraController GetCameraController() => _cameraController;
        public UnityInputControls GetInputControls() => _inputControls;
        public PlayerProperties GetPlayerProperties() => _playerProperties;

    }
}
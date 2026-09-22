using ProjectProvines.Unity.Input;

namespace ProjectProvines.Core.Utils
{
    public class PlayerUtils
    {
        private readonly PlayerInputController _playerInput;
        private readonly CameraController _cameraController;

        public PlayerUtils(PlayerInputController input, CameraController camera)
        {
            _playerInput = input;
            _cameraController = camera;
        }

        public PlayerInputController GetPlayerInput() => _playerInput;
        public CameraController GetCameraController() => _cameraController;
    }
}
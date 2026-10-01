using ProjectProvines.Unity.Input;

namespace ProjectProvines.Core.Utils
{
    public class PlayerUtils
    {
        private readonly PlayerInputController _playerInput;
        private readonly CameraController _cameraController;
        private readonly UnityInputControls _inputControls;

        public PlayerUtils(PlayerInputController input, CameraController camera, UnityInputControls inputControls)
        {
            _playerInput = input;
            _cameraController = camera;
            _inputControls = inputControls;
        }

        public PlayerInputController GetPlayerInput() => _playerInput;
        public CameraController GetCameraController() => _cameraController;
        public UnityInputControls GetInputControls() => _inputControls;
    }
}
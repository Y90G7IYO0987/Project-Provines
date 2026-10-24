using UnityEngine;

namespace ProjectProvines.Core.Player
{
    /// <summary>
    /// The main set of player physics data.
    /// </summary>
    public class PlayerCore
    {
        private readonly float _moveSpeed;
        private readonly float _gravity;
        // Clamping force
        private readonly float _groundedGravity;
        private readonly float _jumpHeight;

        private Vector3 _moveDirection;
        private float _verticalSpeed;
        private float _runMultiplier = 1f;
        private bool _isGrounded;
        private bool _isRunning;

        private float _requiredStartSpeed = 0.12f;

        public bool IsPlayerRunning() => _isRunning && _moveDirection.magnitude > _requiredStartSpeed;

        /// <summary>
        /// Initializes the initial physics parameters.
        /// </summary>
        /// <param name="moveSpeed">Movement speed.</param>
        /// <param name="jumpHeight">Jump height.</param>
        /// <param name="gravity">Gravitational parameter.</param>
        /// <param name="groundGravity">The parameter of the clamping force.</param>
        public PlayerCore(float moveSpeed, float jumpHeight, float gravity = -9.81f, float groundGravity = -2f)
        {
            _moveSpeed = moveSpeed;
            _jumpHeight = jumpHeight;
            _gravity = gravity;
            _groundedGravity = groundGravity;
        }

        /// <summary>
        /// Calculates the player’s horizontal speed.
        /// </summary>
        /// <returns>Horizontal velocity vector</returns>
        public Vector3 CalculateHorizontalVelocity()
        {
            return _moveDirection * _moveSpeed * _runMultiplier;
        }

        /// <summary>
        /// Calculates the player’s falling speed.
        /// </summary>
        /// <returns>Falling speed</returns>
        public float CalculateVerticalVelocity(float deltaTime)
        {
            if (_isGrounded && _verticalSpeed < 0f)
                _verticalSpeed = _groundedGravity;
            _verticalSpeed += _gravity * deltaTime;
            return _verticalSpeed;
        }

        public void TryJump()
        {
            if (!_isGrounded) return;
            _verticalSpeed = Mathf.Sqrt(-2f * _gravity * _jumpHeight);
        }

        public void StartRunning()
        {
            _runMultiplier = 2f;
            _isRunning = true;
        }

        public void StopRunning()
        {
            _runMultiplier = 1f;
            _isRunning = false;
        }

        /// <summary>
        /// Updates the player’s status as being on the ground.
        /// </summary>
        /// <param name="isGrounded">The significance of a player being on the ground.</param>
        public void SetPlayerGrounded(bool isGrounded)
        {
            _isGrounded = isGrounded;
        }

        /// <summary>
        /// Changes the player’s direction of movement.
        /// </summary>
        /// <param name="direction">Direction of movement</param>
        public void SetMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }
    }
}
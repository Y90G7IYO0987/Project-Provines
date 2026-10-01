using UnityEngine;

namespace ProjectProvines.Core.Player
{
    /// <summary>
    /// Основной счет данных для игрока.
    /// </summary>
    public class PlayerCore
    {
        private readonly float _moveSpeed;
        private readonly float _gravity;
        // Прижимная сила
        private readonly float _groundedGravity;
        private readonly float _jumpHeight;

        private Vector3 _moveDirection;
        private float _verticalSpeed;
        private float _runMultiplier = 1f;
        private bool _isGrounded;
        private bool _isRunning;

        private float _requiredStartSpeed = 0.12f;

        public PlayerCore(float moveSpeed, float jumpHeight, float gravity = -9.81f, float groundGravity = -2f)
        {
            _moveSpeed = moveSpeed;
            _jumpHeight = jumpHeight;
            _gravity = gravity;
            _groundedGravity = groundGravity;
        }

        /// <summary>
        /// Считает горизонтальную скорость игрока.
        /// </summary>
        /// <returns>Вектор горизонтальной скорости</returns>
        public Vector3 CalculateHorizontalVelocity()
        {
            return _moveDirection * _moveSpeed * _runMultiplier;
        }

        /// <summary>
        /// Считает скорость падения игрока.
        /// </summary>
        /// <param name="deltaTime">Параметр изменения времени</param>
        /// <returns>Скорость падения</returns>
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
        /// Обновляет статус нахождения игрока на земле.
        /// </summary>
        /// <param name="isGrounded">True, если игрок касается поверхности земли.</param>
        public void SetPlayerGrounded(bool isGrounded)
        {
            _isGrounded = isGrounded;
        }

        /// <summary>
        /// Меняет направление движения игрока.
        /// </summary>
        /// <param name="direction">Направление движения</param>
        public void SetMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }

        public bool IsPlayerRunning() => _isRunning && _moveDirection.magnitude > _requiredStartSpeed;
    }
}
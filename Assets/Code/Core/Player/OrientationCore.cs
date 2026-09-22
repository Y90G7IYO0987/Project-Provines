using UnityEngine;

namespace ProjectProvines.Core.Player
{
    public class OrientationCore
    {
        private readonly float _rotationSpeed;
        private readonly float _smoothTime;

        private float _yawAngle;
        private float _yawVelocity;

        public OrientationCore(float rotationSpeed, float initialYaw, float smoothTime)
        {
            _rotationSpeed = rotationSpeed;
            _yawAngle = initialYaw;
            _smoothTime = smoothTime;
        }

        /// <summary>
        /// Плавно доворачивает yaw угол к целевому углу.
        /// </summary>
        /// <param name="deltaTime">Дельта изменения времени.</param>
        /// <param name="targetYaw">Целевой угол.</param>
        /// <returns>Новый целевой угол.</returns>
        public float CalculateYawAngle(float deltaTime, float targetYaw)
        {
            _yawAngle = Mathf.SmoothDampAngle(_yawAngle, targetYaw, ref _yawVelocity, _smoothTime);
            return _yawAngle;
        }
    }
}
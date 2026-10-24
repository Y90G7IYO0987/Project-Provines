using UnityEngine;

namespace ProjectProvines.Core.Player
{
    /// <summary>
    /// A class for the character’s correct orientation.
    /// Controls the character’s turns.
    /// </summary>
    public class OrientationCore
    {
        private readonly float _smoothTime;

        private float _yawAngle;
        private float _yawVelocity;

        public OrientationCore(float initialYaw, float smoothTime)
        {
            _yawAngle = initialYaw;
            _smoothTime = smoothTime;
        }

        /// <summary>
        /// Smoothly adjusts the horizontal angle to the target one.
        /// </summary>
        /// <param name="targetYaw">Target angle.</param>
        /// <returns>New Angle.</returns>
        public float CalculateYawAngle(float deltaTime, float targetYaw)
        {
            _yawAngle = Mathf.SmoothDampAngle(_yawAngle, targetYaw, ref _yawVelocity, _smoothTime);
            return _yawAngle;
        }
    }
}
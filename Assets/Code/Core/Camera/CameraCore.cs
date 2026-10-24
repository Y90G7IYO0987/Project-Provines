using UnityEngine;

namespace ProjectProvines.Core.Camera
{
    /// <summary>
    /// The main logic of the orbital camera.
    /// </summary>
    public class CameraCore
    {
        public float Yaw => _yaw;

        // Horizontal angle
        private float _yaw;
        // Vertical angle
        private float _pitch;
        // Camera distance from the player
        private float _distance;

        /// <summary>
        /// Sets the base values for angles and distances.
        /// </summary>
        /// <param name="initialYaw">Initial horizontal angle.</param>
        /// <param name="initialPitch">Initial vertical angle.</param>
        /// <param name="defaultDistance">The basic camera distance from the player.</param>
        public CameraCore(float initialYaw, float initialPitch, float defaultDistance)
        {
            _yaw = initialYaw;
            _pitch = initialPitch;
            _distance = defaultDistance;
        }

        /// <summary>
        /// Rotates the camera by the specified deltas.
        /// </summary>
        /// <param name="yawDelta">Change in the horizontal angle.</param>
        /// <param name="pitchDelta">Change in the vertical angle.</param>
        public void Rotate(float yawDelta, float pitchDelta)
        {
            _yaw += yawDelta;
            _pitch -= pitchDelta;
        }

        /// <summary>
        /// It limits the vertical angle to prevent the camera from tipping over.
        /// </summary>
        /// <param name="min">Minimum angle.</param>
        /// <param name="max">Maximum angle.</param>
        public void ClampPitch(float min, float max)
        {
            _pitch = Mathf.Clamp(_pitch, min, max);
        }

        /// <summary>
        /// Changes the camera distance.
        /// </summary>
        /// <param name="scrollDelta">The meaning of scrolling the wheel</param>
        /// <param name="min">Minimum distance.</param>
        /// <param name="max">Maximum distance.</param>
        /// <param name="zoomSpeed">Zoom speed.</param>
        public void Zoom(float scrollDelta, float min, float max, float zoomSpeed)
        {
            _distance -= scrollDelta * zoomSpeed;
            _distance = Mathf.Clamp(_distance, min, max);
        }

        /// <summary>
        /// Sets the distance directly.
        /// </summary>
        public void SetDistance(float distance, float minDistance, float maxDistance)
        {
            _distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }


        /// <summary>
        /// Calculates the camera’s position in the world.
        /// </summary>
        /// <param name="targetCenter">The point around which the camera rotates.</param>
        /// <returns>The camera’s global position.</returns>
        public Vector3 GetCameraPosition(Vector3 targetCenter)
        {
            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
            Vector3 direction = rotation * Vector3.back;
            return targetCenter + direction * _distance;
        }

        /// <summary>
        /// Provides smooth rotation of the camera along the horizontal axis.
        /// </summary>
        /// <param name="targetYaw">Target rotation in degrees</param>
        /// <param name="followSpeed">Rotation speed</param>
        public void FollowYaw(float deltaTime, float targetYaw, float followSpeed)
        {
            float delta = Mathf.DeltaAngle(_yaw, targetYaw);
            float step = followSpeed * deltaTime;
            float clampedDelta = Mathf.Clamp(delta, -step, step);

            _yaw += clampedDelta;
            _yaw = Mathf.Repeat(_yaw, 360f);
        }
    }
}
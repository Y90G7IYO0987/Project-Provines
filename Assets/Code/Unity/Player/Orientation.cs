using ProjectProvines.Core.Player;
using UnityEngine;

namespace ProjectProvines.Unity.Player
{
    /// <summary>
    /// The main handler for player rotation actions.
    /// </summary>
    public class Orientation : MonoBehaviour
    {
        [SerializeField] private float _smoothTime = 0.15f;

        private OrientationCore _orientationCore;

        /// <summary>
        /// Initializes the orientation kernel script.
        /// </summary>
        private void Awake()
        {
            _orientationCore = new OrientationCore(transform.eulerAngles.y, _smoothTime);
        }

        // Describes the character’s smooth rotation.
        public void RotateTowards(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.01f) return;

            float targetYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float newYaw = _orientationCore.CalculateYawAngle(Time.deltaTime, targetYaw);

            transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
        }
    }
}
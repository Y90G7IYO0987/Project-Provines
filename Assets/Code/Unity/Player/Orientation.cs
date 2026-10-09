using ProjectProvines.Core.Player;
using UnityEngine;

namespace ProjectProvines.Unity.Player
{
    public class Orientation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 720f;
        [SerializeField] private float _smoothTime = 0.15f;

        private OrientationCore _orientationCore;

        private void Awake()
        {
            _orientationCore = new OrientationCore(rotationSpeed, transform.eulerAngles.y, _smoothTime);
        }

        // Описывает плавное вращение персонажа.
        public void RotateTowards(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.01f) return;

            float targetYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float newYaw = _orientationCore.CalculateYawAngle(Time.deltaTime, targetYaw);

            transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
        }
    }
}
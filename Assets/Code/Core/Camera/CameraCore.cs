using UnityEngine;

namespace ProjectProvines.Core.Camera
{
    /// <summary>
    /// Основная логика орбитальной камеры.
    /// </summary>
    public class CameraCore
    {
        public float Yaw => _yaw;

        // Горизонтальный угол
        private float _yaw;
        // Вертикальный угол
        private float _pitch;
        // Дистанция камеры от игрока
        private float _distance;

        /// <summary>
        /// Устанавливает базовые значения углам и дистанции.
        /// </summary>
        /// <param name="initialYaw">Начальный горизонтальный угол.</param>
        /// <param name="initialPitch">Начальный вертикальный угол.</param>
        /// <param name="defaultDistance">Базовая дистанция камеры от игрока.</param>
        public CameraCore(float initialYaw, float initialPitch, float defaultDistance)
        {
            _yaw = initialYaw;
            _pitch = initialPitch;
            _distance = defaultDistance;
        }

        /// <summary>
        /// Вращает камеру на заданные дельты.
        /// </summary>
        /// <param name="yawDelta">Изменение горизонтального угла.</param>
        /// <param name="pitchDelta">Изменение вертикального угла.</param>
        public void Rotate(float yawDelta, float pitchDelta)
        {
            _yaw += yawDelta;
            _pitch -= pitchDelta;
        }

        /// <summary>
        /// Ограничивает вертикальный угол, чтобы камера не переворачивалась.
        /// </summary>
        /// <param name="min">Минимальный угол.</param>
        /// <param name="max">Максимальный угол.</param>
        public void ClampPitch(float min, float max)
        {
            _pitch = Mathf.Clamp(_pitch, min, max);
        }

        /// <summary>
        /// Изменяет дистанцию камеры.
        /// </summary>
        /// <param name="scrollDelta">Значение прокрутки колесика</param>
        /// <param name="min">Минимальная дистанция.</param>
        /// <param name="max">Максимальная дистанция.</param>
        /// <param name="zoomSpeed">Скорость зума.</param>
        public void Zoom(float scrollDelta, float min, float max, float zoomSpeed)
        {
            _distance -= scrollDelta * zoomSpeed;
            _distance = Mathf.Clamp(_distance, min, max);
        }

        /// <summary>
        /// Устанавливает дистанцию напрямую.
        /// </summary>
        public void SetDistance(float distance, float minDistance, float maxDistance)
        {
            _distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }


        /// <summary>
        /// Вычисляет позицию камеры в мире.
        /// </summary>
        /// <param name="targetCenter">Точка, вокруг которой вращается камера.</param>
        /// <param name="heightOffset">Смещение вверх от цели камеры.</param>
        /// <returns>Мировая позиция камеры.</returns>
        public Vector3 GetCameraPosition(Vector3 targetCenter)
        {
            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
            Vector3 direction = rotation * Vector3.back;
            return targetCenter + direction * _distance;
        }

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
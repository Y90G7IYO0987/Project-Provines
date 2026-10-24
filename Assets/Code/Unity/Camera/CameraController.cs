namespace ProjectProvines.Unity.Camera
{
    using ProjectProvines.Core.Camera;
    using ProjectProvines.Unity.Scriptables;
    using UnityEngine;

    /// <summary>
    /// Controls the camera’s main functions.
    /// It connects the camera core to Unity.
    /// </summary>
    [RequireComponent(typeof(PlayerData))]
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private PlayerData playerData;
        [SerializeField] private float heightOffset = 1.5f;

        [Header("Rotation")]
        [SerializeField] private float mouseSensitivity = 1.25f;
        [SerializeField] private float pitchMin = -20f;
        [SerializeField] private float pitchMax = 80f;

        [Header("Distance")]
        [SerializeField] private float defaultDistance = 3.3f;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxDistance = 5f;
        [SerializeField] private float zoomSpeed = 2f;

        [Header("Smooth")]
        [SerializeField] private float smoothTime = 0.08f;

        [Header("Collision")]
        [SerializeField] private LayerMask collisionLayers = -1;
        [SerializeField] private float collisionRadius = 0.3f;

        [Header("Auto Follow")]
        [SerializeField] private bool _autoFollowEnabled = true;
        [SerializeField] private float _autoFollowSpeed = 120f;
        [SerializeField] private float _autoFollowDelay = 1.0f;
        [SerializeField] private float _autoFollowThreshold = 2f;

        private CameraCore _cameraCore;
        private GameObject _target;

        private Vector3 _smoothVelocity;
        private float _lastMouseInputTime;

        /// <summary>
        /// Initialization of the camera core
        /// </summary>
        private void Awake()
        {
            Vector3 angles = transform.eulerAngles;

            _cameraCore = new CameraCore(angles.y, angles.x, defaultDistance);
        }

        private void Start()
        {
            _target = playerData.Prefab;
        }

        private void Update()
        {
            HandleRotation();
            HandleZoom();
        }

        /// <summary>
        /// Combines the main functions of the camera.
        /// </summary>
        private void LateUpdate()
        {
            if (_target == null)
                return;

            AutoFollow();
            HandleCollision();
            HandlePosition();
        }

        /// <summary>
        /// Camera rotation while holding down the right‑mouse button.
        /// </summary>
        private void HandleRotation()
        {
            if (!Input.GetMouseButton(1)) return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
                _lastMouseInputTime = Time.time;

            _cameraCore.Rotate(mouseX, mouseY);
            _cameraCore.ClampPitch(pitchMin, pitchMax);
        }

        /// <summary>
        /// Zoom with the mouse wheel.
        /// </summary>
        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) < 0.01f) return;

            _cameraCore.Zoom(scroll, minDistance, maxDistance, zoomSpeed);
        }

        /// <summary>
        /// Collision check: if there’s a wall between the player and the camera, move closer.
        /// </summary>
        private void HandleCollision()
        {
            Vector3 targetCenter = _target.transform.position + Vector3.up * heightOffset;
            Vector3 desiredPosition = _cameraCore.GetCameraPosition(targetCenter);
            Vector3 direction = (desiredPosition - targetCenter).normalized;
            float desiredDistance = Vector3.Distance(targetCenter, desiredPosition);

            if (Physics.SphereCast(targetCenter, collisionRadius, direction,
                out RaycastHit hit, desiredDistance, collisionLayers))
            {
                if (hit.transform != _target.transform)
                {
                    float hitDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
                    _cameraCore.SetDistance(hitDistance, minDistance, maxDistance);
                }
            }
        }

        /// <summary>
        /// Turning and viewing the camera behind the player.
        /// </summary>
        private void HandlePosition()
        {
            Vector3 targetCenter = _target.transform.position + Vector3.up * heightOffset;
            Vector3 desiredPosition = _cameraCore.GetCameraPosition(targetCenter);

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _smoothVelocity, smoothTime);

            transform.LookAt(targetCenter);
        }

        /// <summary>
        /// Slowly turns the camera behind the character.
        /// </summary>
        private void AutoFollow()
        {
            if (!_autoFollowEnabled) return;
            if (_target == null) return;

            if (Time.time - _lastMouseInputTime < _autoFollowDelay) return;

            float targetYaw = _target.transform.eulerAngles.y;

            float delta = Mathf.Abs(Mathf.DeltaAngle(_cameraCore.Yaw, targetYaw));
            if (delta < _autoFollowThreshold) return;

            _cameraCore.FollowYaw(Time.deltaTime, targetYaw, _autoFollowSpeed);
        }
    }
}
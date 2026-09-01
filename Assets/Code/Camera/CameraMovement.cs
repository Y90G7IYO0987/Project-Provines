using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    [Header("Rotation Settings")]
    [SerializeField] private float horizontalSpeed = 120.0f;
    [SerializeField] private float verticalSpeed = 120.0f;
    [SerializeField] private float verticalMinSight = -20.0f;
    [SerializeField] private float verticalMaxSight = 80.0f;

    [Header("Distance Settings")]
    [SerializeField] private float minDistance = 1.0f;
    [SerializeField] private float maxDistance = 10.0f;
    [SerializeField] private float defaultDistance = 3.0f;

    [Header("Smooth Settings")]
    [SerializeField] private float smoothSpeed = 10f;

    [Header("Collision")]
    [SerializeField] private LayerMask collisionLayers = -1;

    private GameObject _target;
    private float _currentX;
    private float _currentY;
    private float _currentXVelocity;
    private float _currentYVelocity;
    private float _currentDistance;
    private Vector3 _smoothVelocity;

    private void Awake()
    {
        InitializeAngles();
        _currentDistance = defaultDistance;
    }

    private void Start()
    {
        _target = playerData.Prefab;
    }

    private void Update()
    {
        if (_target == null) return;

        if (Input.GetMouseButton(1))
        {
            float horizontalAxis = Input.GetAxis("Mouse X");
            float verticalAxis = Input.GetAxis("Mouse Y");

            float targetX = _currentX + verticalAxis * horizontalSpeed * Time.deltaTime;
            float targetY = _currentY - horizontalAxis * verticalSpeed * Time.deltaTime;

            _currentX = Mathf.SmoothDamp(_currentX, targetX, ref _currentXVelocity, 0.1f);
            _currentY = Mathf.SmoothDamp(_currentY, targetY, ref _currentYVelocity, 0.1f);

            _currentY = Mathf.Clamp(_currentY, verticalMinSight, verticalMaxSight);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _currentDistance -= scroll * 3f;
        _currentDistance = Mathf.Clamp(_currentDistance, minDistance, maxDistance);
    }

    private void LateUpdate()
    {
        if (playerData == null || playerData.Prefab == null) return;

        Quaternion rotation = Quaternion.Euler(_currentX, _currentY, 0);

        Vector3 targetCenter = _target.transform.position + Vector3.up * 1.5f;
        Vector3 desiredPosition = targetCenter - rotation * Vector3.forward * _currentDistance;

        float targetDistance = _currentDistance;

        RaycastHit hit;
        if (Physics.Linecast(targetCenter, desiredPosition, out hit, collisionLayers))
        {
            targetDistance = Mathf.Clamp(hit.distance - 0.2f, minDistance, maxDistance);
            desiredPosition = targetCenter - rotation * Vector3.forward * targetDistance;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref _smoothVelocity,
            1f / smoothSpeed
            );

        transform.LookAt(targetCenter);
    }

    private void InitializeAngles()
    {
        Vector3 angles = transform.eulerAngles;
        _currentX = angles.x;
        _currentY = angles.y;
    }
}

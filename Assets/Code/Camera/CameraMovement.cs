using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 3.0f;
    [SerializeField] private Vector3 _cameraOffset = new Vector3(0, 3.5f, -1.5f);
    [SerializeField] private Vector3 _cameraRotation = new Vector3(50f, 0, 0);

    private PlayerData _playerData;
    private GameObject _player;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(_cameraRotation.x, _cameraRotation.y, _cameraRotation.z);
    }

    private void LateUpdate()
    {
        if (_playerData == null || _playerData.Prefab == null) return;

        Vector3 targetPosition = _player.transform.position + _cameraOffset;
        Vector3 cameraPosition = Vector3.Lerp(transform.position, targetPosition, (_movementSpeed * Time.deltaTime));

        transform.position = cameraPosition;
        Debug.Log($"Camera position - {cameraPosition}");
    }

    public void SetPlayerData(PlayerData playerData)
    {
        _playerData = playerData;
        _player = playerData.Prefab;
    }
}

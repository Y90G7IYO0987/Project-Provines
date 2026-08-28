using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private readonly int XPositionHash = Animator.StringToHash(XPosition);
    private readonly int ZPositionHash = Animator.StringToHash(ZPosition);
    private readonly int IsRunningHash = Animator.StringToHash(IsRunning);
    private readonly int IsGroundedHash = Animator.StringToHash(IsGrounded);

    [SerializeField] private PlayerData playerData;

    private Animator _animator;
    private GameObject _player;
    private PlayerPhysics _playerPhysics;
    private Vector3 _moveDirection;

    private const string XPosition = "XPosition";
    private const string ZPosition = "ZPosition";
    private const string IsRunning = "IsRunning";
    private const string IsGrounded = "IsGrounded";

    private void SetRunningAnimateStates() => _animator.SetBool(IsRunningHash, _playerPhysics.IsPlayerRunning());
    private void SetJumpAnimateStates() => _animator.SetBool(IsGroundedHash, _playerPhysics.IsGroundedPlayer());

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _player = playerData.Prefab;
        _playerPhysics = _player.GetComponent<PlayerPhysics>();
    }

    private void Update()
    {
        _moveDirection = _playerPhysics.MoveDirection;
        Debug.Log($"Move direction - {_moveDirection}");

        SetMovingAnimateStates();
        SetRunningAnimateStates();

        SetJumpAnimateStates();
    }

    private void SetMovingAnimateStates()
    {
        _animator.SetFloat(XPositionHash, _moveDirection.x);
        _animator.SetFloat(ZPositionHash, _moveDirection.z);
    }    
}

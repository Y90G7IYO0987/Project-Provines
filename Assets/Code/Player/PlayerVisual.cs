using System.Collections;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private readonly int XPositionHash = Animator.StringToHash(XPosition);
    private readonly int ZPositionHash = Animator.StringToHash(ZPosition);
    private readonly int IsRunningHash = Animator.StringToHash(IsRunning);
    private readonly int IsGroundedHash = Animator.StringToHash(IsGrounded);

    public float CurrentHealth { get; private set; }
    public float CurrentStamina { get; private set; }

    [SerializeField] private PlayerData playerData;
    [SerializeField] private float changeStaminaCount = 1.15f;
    [SerializeField] private float chagingStaminaDebounceTime = 0.35f;
    [SerializeField] private float regeningStaminaCount = 50.0f;
    [SerializeField] private float regeningStaminaDebounceTime = 0.3f;
    [SerializeField] private float startingRegenTime = 0.85f;

    private Animator _animator;
    private GameObject _player;
    private PlayerPhysics _playerPhysics;
    private Vector3 _moveDirection;
    private bool _startedStaminaRoutine;
    private bool _isRunning;
    private bool _staminaRegening;

    private const string XPosition = "XPosition";
    private const string ZPosition = "ZPosition";
    private const string IsRunning = "IsRunning";
    private const string IsGrounded = "IsGrounded";

    public void SetRunning(bool running) => _isRunning = running;
    private void SetRunningAnimateStates() => _animator.SetBool(IsRunningHash, _playerPhysics.IsPlayerRunning());
    private void SetJumpAnimateStates() => _animator.SetBool(IsGroundedHash, _playerPhysics.IsGroundedPlayer());

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        CurrentHealth = playerData.MaxHealth;
        CurrentStamina = playerData.MaxStamina;
    }

    private void Start()
    {
        _player = playerData.Prefab;
        _playerPhysics = _player.GetComponent<PlayerPhysics>();        
    }

    private void Update()
    {
        _moveDirection = _playerPhysics.MoveDirection;

        SetMovingAnimateStates();
        SetRunningAnimateStates();
        SetJumpAnimateStates();

        StaminaSliderManagement();
    }

    public void ChangeHealth(float amount)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, playerData.MaxHealth);
    }

    private void SetMovingAnimateStates()
    {
        _animator.SetFloat(XPositionHash, _moveDirection.x);
        _animator.SetFloat(ZPositionHash, _moveDirection.z);
    }    

    private void StaminaSliderManagement()
    {
        if (!_isRunning && CurrentStamina < playerData.MaxStamina && !_staminaRegening)
        {
            StartCoroutine(StaminaRegeningRoutine());
        }

        if (CurrentStamina == 0f)
        {
            _playerPhysics.StoppingRun();
            return;
        }        

        if (!_isRunning) return;
        if (_startedStaminaRoutine) return;

        if (_isRunning && _playerPhysics.HorizontalSpeed.magnitude < _playerPhysics.RequireMoveLength)
        {
            _playerPhysics.StoppingRun();
            return;
        }

        StartCoroutine(StaminaMovementRoutine());
    }

    private IEnumerator StaminaRegeningRoutine()
    {
        _staminaRegening = true;

        yield return new WaitForSeconds(startingRegenTime);

        while (!_isRunning && CurrentStamina < (playerData.MaxStamina * 25 / 100))
        {
            CurrentStamina += (regeningStaminaCount * 2 ) * Time.deltaTime;

            yield return new WaitForSeconds(regeningStaminaDebounceTime);
        }

        while (!_isRunning && CurrentStamina >= (playerData.MaxStamina * 25 / 100))
        {
            CurrentStamina += (regeningStaminaCount * 2) * Time.deltaTime;

            yield return new WaitForSeconds(regeningStaminaDebounceTime);
        }

        _staminaRegening = false;
    }

    private IEnumerator StaminaMovementRoutine()
    {
        _startedStaminaRoutine = true;        

        while (_isRunning && CurrentStamina > 0f && _playerPhysics.HorizontalSpeed.magnitude > _playerPhysics.RequireMoveLength)
        {
            CurrentStamina -= changeStaminaCount;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, playerData.MaxStamina);

            yield return new WaitForSeconds(chagingStaminaDebounceTime);
        }

        _startedStaminaRoutine = false;
    }
}

using UnityEngine;
using UnityEngine.UI;

public struct SlidersData
{
    public Slider HealthSlider;
    public Slider StaminaSlider;
    public PlayerData PlayerData;
    public float SlidersUpdateSpeed;
}

public class BarSliders : MonoBehaviour
{
    private Slider _healthSlider;
    private Slider _staminaSlider;    
    private PlayerData _playerData;

    private float _slidersUpdateSpeed;
    private bool _isHealthUpdated;
    private bool _isStaminaUpdated;

    private void Update()
    {
        _isHealthUpdated = _healthSlider.value != _playerData.CurrentHealth ? true : false;
        _isStaminaUpdated = _staminaSlider.value != _playerData.CurrentStamina ? true : false;

        if (_isHealthUpdated) _healthSlider.value = Mathf.MoveTowards(_healthSlider.value, _playerData.CurrentHealth, Time.deltaTime * _slidersUpdateSpeed);

        if (_isStaminaUpdated) _staminaSlider.value = Mathf.MoveTowards(_staminaSlider.value, _playerData.CurrentStamina, Time.deltaTime * _slidersUpdateSpeed);

        if (_isHealthUpdated || _isStaminaUpdated) Debug.Log($"Some sliders updated: \n\n Current slider values:\n\n Health Slider value -> {_healthSlider.value},\n\n Stamina Slider value -> {_staminaSlider.value}.");
    }

    public void Initialize(SlidersData slidersData)
    {
        _healthSlider = slidersData.HealthSlider;
        _staminaSlider = slidersData.StaminaSlider;
        _playerData = slidersData.PlayerData;
        _slidersUpdateSpeed = slidersData.SlidersUpdateSpeed;
    }
}

public class AnimationsConfigure : MonoBehaviour
{
    public readonly int RunningHash = Animator.StringToHash(Running);

    private Animator _animator;
    private PlayerData _playerData;

    private const string Running = "IsRunning";
    private bool _isRunning;

    private void Update()
    {
        _animator.SetBool(RunningHash, _isRunning);        
    }

    public void Initialize(PlayerData playerData)
    {
        _playerData = playerData;
        _animator = playerData.Animator;
        _isRunning = _playerData.IsRunning;
    }
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float slidersUpdateSpeed = 3.0f;
    [SerializeField] private PlayerData playerData;

    private SlidersData _receiverData;
    private BarSliders _barSliders;
    private AnimationsConfigure _animationsConfigure;

    private void Awake()
    {
        _receiverData = new SlidersData
        {
            HealthSlider = healthSlider,
            StaminaSlider = staminaSlider,
            SlidersUpdateSpeed = slidersUpdateSpeed,
            PlayerData = playerData
        };

        _barSliders = gameObject.AddComponent<BarSliders>();
        _animationsConfigure = gameObject.AddComponent<AnimationsConfigure>();
    }

    void Start()
    {
        _barSliders.Initialize(_receiverData);
        _animationsConfigure.Initialize(playerData);
    }
}

using ProjectProvines.Core.Player;
using UnityEngine;
using UnityEngine.UI;

public class BarSliders : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float sliderUpdateSpeed = 2.5f;
    [SerializeField] private Slider[] barSliders = new Slider[3];

    private Slider _healthSlider;
    private Slider _magicSlider;
    private Slider _staminaSlider;

    private PlayerProperties _playerProperties;

    private float _maxHealth;
    private float _maxMagic;
    private float _maxStamina;

    private bool _anyChanges;

    private void Awake()
    {
        _maxHealth = playerData.MaxHealth;
        _maxMagic = playerData.MaxMagic;
        _maxStamina = playerData.MaxStamina;

        if (barSliders.Length > 0)
        {
            _healthSlider = barSliders[0];
            _magicSlider = barSliders[1];
            _staminaSlider = barSliders[2];
        }
    }

    private void Start()
    {
        _playerProperties = playerData.PlayerUtils.GetPlayerProperties();
    }

    private void Update()
    {
        _anyChanges =
            _healthSlider.value != _playerProperties.CurrentHealth / _maxHealth ||
            _magicSlider.value != _playerProperties.CurrentMagic / _maxMagic ||
            _staminaSlider.value != _playerProperties.CurrentStamina / _maxStamina
            ? true : false;

        if (_anyChanges)
        {
            _healthSlider.value = Mathf.MoveTowards(
                _healthSlider.value,
                _playerProperties.CurrentHealth / _maxHealth,
                Time.deltaTime * sliderUpdateSpeed);

            _magicSlider.value = Mathf.MoveTowards(
                _magicSlider.value,
                _playerProperties.CurrentMagic / _maxMagic,
                Time.deltaTime * sliderUpdateSpeed);

            _staminaSlider.value = Mathf.MoveTowards(
                _staminaSlider.value,
                _playerProperties.CurrentStamina / _maxStamina,
                Time.deltaTime * sliderUpdateSpeed);
        }
    }

    public void TakeDamage(float amount)
    {
        _playerProperties.AddHealth(-amount);
    }

    public void AddMagic(float amount)
    {
        _playerProperties.AddMagic(amount);
    }

    public void AddStamina(float amount)
    {
        _playerProperties.AddStamina(amount);
    }
}

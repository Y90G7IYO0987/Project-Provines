using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManagement : MonoBehaviour
{
    [SerializeField] private Slider[] barSliders = new Slider[1];
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float updateSpeed = 2.0f;

    private float _healthCount;
    private float _staminaCount;
    private float _maxHealthCount;
    private float _maxStaminaCount;
    private Slider _healthSlider;
    private Slider _staminaSlider;
    private bool _anyCountChanges;

    private GameObject _player;
    private PlayerVisual _playerVisual;

    private void Awake()
    {
        _healthSlider = barSliders[0];
        _staminaSlider = barSliders[1];

        _maxHealthCount = playerData.MaxHealth;
        _maxStaminaCount = playerData.MaxStamina;

        _player = playerData.Prefab;
        _playerVisual = _player.GetComponent<PlayerVisual>();
    }

    private void Update()
    {
        _healthCount = _playerVisual.CurrentHealth;
        _staminaCount = _playerVisual.CurrentStamina;

        _anyCountChanges = (_healthSlider.value * _maxHealthCount) != _healthCount || (_staminaSlider.value * _maxStaminaCount) != _staminaCount;

        if (_anyCountChanges)
        {
            _healthSlider.value = Mathf.MoveTowards(_healthSlider.value, _healthCount / _maxHealthCount, (updateSpeed * Time.deltaTime));
            _staminaSlider.value = Mathf.MoveTowards(_staminaSlider.value, _staminaCount / _maxStaminaCount, (updateSpeed * Time.deltaTime));
        }
    }
}

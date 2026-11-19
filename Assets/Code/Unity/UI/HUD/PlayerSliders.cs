using ProjectProvines.Core.Player;
using ProjectProvines.Core.System;
using ProjectProvines.Unity.Player;
using ProjectProvines.Unity.Scriptables;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    /// <summary>
    /// Describes the behavior of the player’s HUD sliders.
    /// </summary>
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(Slider[]))]
    public class PlayerSliders : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private float sliderUpdateSpeed = 2.5f;
        [SerializeField] private Slider[] playerSliders = new Slider[3];

        [SerializeField] private float StaminaModifierDelay = 0.4f;
        [SerializeField] private float StartingTimeDelay = 0.8f;

        private Slider _healthSlider;
        private Slider _magicSlider;
        private Slider _staminaSlider;

        private PlayerPhysics _playerPhysics;

        private HealthSystem _healthSystem;
        private MagicSystem _magicSystem;
        private StaminaSystem _staminaSystem;

        private float _maxHealth;
        private float _maxMagic;
        private float _maxStamina;

        private bool _anyChanges;
        private bool _isInProgress;

        /// <summary>
        /// Initialization of sliders.
        /// </summary>
        private void Awake()
        {
            _maxHealth = playerData.MaxHealth;
            _maxMagic = playerData.MaxMagic;
            _maxStamina = playerData.MaxStamina;

            _healthSystem = new HealthSystem(_maxHealth);
            _magicSystem = new MagicSystem(_maxMagic);

            playerData.HealthSystem = _healthSystem;

            if (playerSliders.Length == 0)
                return;

            _healthSlider = playerSliders[0];
            _magicSlider = playerSliders[1];
            _staminaSlider = playerSliders[2];
        }

        private void Start()
        {
            _playerPhysics = playerData.Prefab.GetComponent<PlayerPhysics>();
            _staminaSystem = _playerPhysics.GetStaminaSystem();
        }

        /// <summary>
        /// Handles the behavior of sliders.
        /// </summary>
        private void Update()
        {
            if (_healthSlider == null || _magicSlider == null || _staminaSlider == null)
                return;

            _anyChanges =
                _healthSlider.value != _healthSystem.CurrentHealth / _maxHealth ||
                _magicSlider.value != _magicSystem.CurrentMagic / _maxMagic ||
                _staminaSlider.value != _staminaSystem.CurrentStamina / _maxStamina
                ? true : false;

            if (_anyChanges)
            {
                _healthSlider.value = Mathf.MoveTowards(
                    _healthSlider.value,
                    _healthSystem.CurrentHealth / _maxHealth,
                    Time.deltaTime * sliderUpdateSpeed);

                _magicSlider.value = Mathf.MoveTowards(
                    _magicSlider.value,
                    _magicSystem.CurrentMagic / _maxMagic,
                    Time.deltaTime * sliderUpdateSpeed);

                _staminaSlider.value = Mathf.MoveTowards(
                    _staminaSlider.value,
                    _staminaSystem.CurrentStamina / _maxStamina,
                    Time.deltaTime * sliderUpdateSpeed);
            }
        }

        /// <summary>
        /// Deals damage to the player, reduces health.
        /// </summary>
        /// <param name="amount">Change factor.</param>
        public void TakeDamage(float amount)
        {
            _healthSystem.TakeDamage(amount);
        }

        public void AddMagic(float amount)
        {
            _magicSystem.ModifyMagic(amount);
        }

        public void StaminaModifier()
        {
            if (_isInProgress)
                return;

            _staminaSystem.StartModifyStamina();
        }
    }
}
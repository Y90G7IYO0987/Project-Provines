using ProjectProvines.Core.Player;
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
    public class BarSliders : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private float sliderUpdateSpeed = 2.5f;
        [SerializeField] private Slider[] barSliders = new Slider[3];

        [SerializeField] private float DefaultStaminaModifier = 0.75f;
        [SerializeField] private float StaminaModifierDelay = 0.4f;
        [SerializeField] private float StartingTimeDelay = 0.8f;

        private Slider _healthSlider;
        private Slider _magicSlider;
        private Slider _staminaSlider;

        private PlayerProperties _playerProperties;
        private PlayerPhysics _playerPhysics;

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
            _playerPhysics = playerData.Prefab.GetComponent<PlayerPhysics>();
        }

        /// <summary>
        /// Handles the behavior of sliders.
        /// </summary>
        private void Update()
        {
            if (_healthSlider == null || _magicSlider == null || _staminaSlider == null)
                return;

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

        /// <summary>
        /// Deals damage to the player, reduces health.
        /// </summary>
        /// <param name="amount">Change factor.</param>
        public void TakeDamage(float amount)
        {
            if (_playerProperties.CurrentHealth > 0f)
                _playerProperties.TakeDamage(amount);
        }

        public void AddMagic(float amount)
        {
            _playerProperties.AddMagic(amount);
        }

        public void StaminaModifier()
        {
            if (_isInProgress)
                return;

            StartCoroutine(StaminaModifierRoutine());
        }

        private IEnumerator StaminaModifierRoutine()
        {
            _isInProgress = true;

            yield return new WaitForSeconds(StartingTimeDelay);

            while (_playerPhysics.IsRunning)
            {
                _playerProperties.AddStamina(-DefaultStaminaModifier);
                yield return new WaitForSeconds(StaminaModifierDelay);
            }

            _isInProgress = false;
        }
    }
}
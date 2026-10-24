using System;
using UnityEngine;

namespace ProjectProvines.Core.Player
{
    /// <summary>
    /// Player statistics class.
    /// It describes the player’s health, stamina, and magic.
    /// </summary>
    public class PlayerProperties
    {
        public event Action OnPlayerDeath;

        public float CurrentHealth { get; private set; }
        public float CurrentMagic { get; private set; }
        public float CurrentStamina { get; private set; }

        private readonly float _maxHealth;
        private readonly float _maxMagic;
        private readonly float _maxStamina;

        public PlayerProperties(float maxHealth, float maxMagic, float maxStamina)
        {
            _maxHealth = maxHealth;
            _maxMagic = maxMagic;
            _maxStamina = maxStamina;

            CurrentHealth = _maxHealth;
            CurrentMagic = _maxMagic;
            CurrentStamina = _maxStamina;
        }

        /// <summary>
        /// Deals damage and reduces the player’s health.
        /// </summary>
        /// <param name="amount">Change factor.</param>
        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, _maxHealth);

            if (CurrentHealth <= 0f)
                OnPlayerDeath?.Invoke();
        }

        /// <summary>
        /// Changes the player’s magic amount.
        /// </summary>
        /// <param name="amount">Change factor.</param>
        public void AddMagic(float amount)
        {
            CurrentMagic += amount;
            CurrentMagic = Mathf.Clamp(CurrentMagic, 0f, _maxMagic);            
        }

        /// <summary>
        /// Changes the player’s stamina.
        /// </summary>
        /// <param name="amount">Change factor.</param>
        public void AddStamina(float amount)
        {
            CurrentStamina += amount;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, _maxStamina);
        }
    }
}
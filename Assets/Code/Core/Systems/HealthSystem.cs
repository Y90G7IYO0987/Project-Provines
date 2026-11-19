using System;

namespace ProjectProvines.Core.System
{
    /// <summary>
    /// A class for managing the object’s health.
    /// </summary>
    public class HealthSystem
    {
        public event Action OnDeath;

        public float CurrentHealth { get; private set; }

        private readonly float _maxHealth;

        public HealthSystem(float maxHealth)
        {
            _maxHealth = maxHealth;
            CurrentHealth = _maxHealth;
        }

        /// <summary>
        /// Deals damage to the object. Deducts health.
        /// </summary>
        /// <param name="amount">The amount of damage dealt.</param>
        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            CurrentHealth = Math.Clamp(CurrentHealth, 0f, _maxHealth);

            Console.WriteLine($"Damage: {amount}; CurrentHealth: {CurrentHealth}");

            if (CurrentHealth <= 0f)
                OnDeath?.Invoke();
        }
    }
}

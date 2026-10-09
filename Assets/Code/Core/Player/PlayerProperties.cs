using UnityEngine;

namespace ProjectProvines.Core.Player
{
    public class PlayerProperties
    {

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

        public void AddHealth(float amount)
        {
            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, _maxHealth);
        }

        public void AddMagic(float amount)
        {
            CurrentMagic += amount;
            CurrentMagic = Mathf.Clamp(CurrentMagic, 0f, _maxMagic);            
        }

        public void AddStamina(float amount)
        {
            CurrentStamina += amount;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, _maxStamina);
        }
    }
}
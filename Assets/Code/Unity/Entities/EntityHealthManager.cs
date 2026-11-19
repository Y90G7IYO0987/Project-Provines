using ProjectProvines.Core.System;
using ProjectProvines.Unity.Scriptables;
using UnityEngine;

namespace ProjectProvines.Unity.Entities
{
    public class EntityHealthManager : MonoBehaviour
    {
        public float CurrentHealth => _healthSystem.CurrentHealth;
        public float MaxHealth => entityData.MaxHealth;

        [SerializeField] private EntityData entityData;

        private HealthSystem _healthSystem;

        private void Awake()
        {
            _healthSystem = new HealthSystem(entityData.MaxHealth);
        }

        public void TakeDamage(float amount)
        {
            _healthSystem.TakeDamage(amount);

            if (_healthSystem.CurrentHealth == 0f)
                Destroy(gameObject);
        }
    }
}
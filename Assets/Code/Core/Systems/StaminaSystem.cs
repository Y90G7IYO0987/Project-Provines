using ProjectProvines.Unity.Player;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectProvines.Core.System
{
    /// <summary>
    /// A class that describes the behavior of an object’s stamina.
    /// </summary>
    public class StaminaSystem
    {
        public Single CurrentStamina { get; private set; }

        private readonly Single _maxStamina;
        private readonly Single _staminaModifierPercent = 0.11f;
        private readonly Single _staminaModifierInterval = 0.75f;

        private readonly Single _regeningPercent = 0.15f;
        private readonly Single _cooldownBeforeRegening = 0.8f;

        private readonly PlayerPhysics _physics;

        private Boolean _isRunning => _physics.IsRunning;

        public StaminaSystem(Single maxStamina, PlayerPhysics physics)
        {
            _maxStamina = maxStamina;
            CurrentStamina = _maxStamina;

            _physics = physics;
        }

        /// <summary>
        /// Reduces stamina.
        /// </summary>
        public void StartModifyStamina()
        {
            _ = ModifyStamina();
        }

        /// <summary>
        /// The basic logic of stamina subtraction.
        /// </summary>
        private IEnumerator ModifyStamina()
        {
            while (CurrentStamina > 0f && _isRunning)
            {
                CurrentStamina -= _maxStamina * _staminaModifierPercent;

                yield return new WaitForSeconds(_staminaModifierInterval);
            }

            if (CurrentStamina < _maxStamina)
                _ = RegeningStamina();
        }

        /// <summary>
        /// The logic of health regeneration.
        /// </summary>
        private IEnumerator RegeningStamina()
        {
            yield return new WaitForSeconds(_cooldownBeforeRegening);

            while (CurrentStamina < (_maxStamina * _regeningPercent) && !_isRunning)
            {
                CurrentStamina += _maxStamina * _regeningPercent;
                yield return new WaitForSeconds(_staminaModifierInterval);
            }
        }
    }
}
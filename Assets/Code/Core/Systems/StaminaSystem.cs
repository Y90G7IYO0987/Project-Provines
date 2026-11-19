using ProjectProvines.Unity.Player;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectProvines.Core.System
{
    /// <summary>
    /// A class that describes the behavior of an object’s stamina.
    /// </summary>
    public class StaminaSystem
    {
        public float CurrentStamina { get; private set; }

        private readonly float _maxStamina;
        private readonly float _staminaModifierPercent = 0.11f;
        private readonly int _staminaModifierInterval = 750;

        private readonly float _regeningPercent = 0.15f;
        private readonly int _cooldownBeforeRegening = 800;

        private readonly PlayerPhysics _physics;

        private bool _isRunning => _physics.IsRunning;

        public StaminaSystem(float maxStamina, PlayerPhysics physics)
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
        private async Task ModifyStamina()
        {
            while (CurrentStamina > 0f && _isRunning)
            {
                CurrentStamina -= _maxStamina * _staminaModifierPercent;

                await Task.Delay(_staminaModifierInterval);
            }

            if (CurrentStamina < _maxStamina)
                _ = RegeningStamina();
        }

        /// <summary>
        /// The logic of health regeneration.
        /// </summary>
        private async Task RegeningStamina()
        {
            await Task.Delay(_cooldownBeforeRegening);

            while (CurrentStamina < (_maxStamina * _regeningPercent) && !_isRunning)
            {
                CurrentStamina += _maxStamina * _regeningPercent;

                await Task.Delay(_staminaModifierInterval);
            }
        }
    }
}
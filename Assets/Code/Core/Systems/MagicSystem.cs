using System;
using System.Collections;
using System.Threading.Tasks;

namespace ProjectProvines.Core.System
{
    /// <summary>
    /// A class that implements the magic of an object.
    /// </summary>
    public class MagicSystem
    {
        public float CurrentMagic { get; private set; }

        private readonly float _maxMagic;
        private readonly float _modifyMagicCount = 0.18f;

        private readonly int _regenerationIntervalMS = 600;
        private readonly int _cooldownBeforeRegeneration = 2400;
        private readonly float _regenerationThreshold = 0.35f;

        private bool _isRegening;

        public MagicSystem(float maxMagic)
        {
            _maxMagic = maxMagic;
            CurrentMagic = _maxMagic;
        }

        /// <summary>
        /// Changes the magic to a new specified amount.
        /// </summary>
        /// <param name="amount">The amount of new magic.</param>
        public void ModifyMagic(float amount)
        {
            CurrentMagic -= amount;
            CurrentMagic = Math.Clamp(CurrentMagic, 0f, _maxMagic);

            if (CurrentMagic < (_maxMagic * _regenerationThreshold))
                UpdateMagic();
        }

        /// <summary>
        /// Updates magic. Refills the magic supply.
        /// </summary>
        private void UpdateMagic()
        {
            if (_isRegening)
                return;

            _ = RegenerateMagic();
        }

        /// <summary>
        /// Replenishes the magic supply over a certain period of time.
        /// </summary>
        private async Task RegenerateMagic()
        {
            await Task.Delay(_cooldownBeforeRegeneration);

            _isRegening = true;

            while (CurrentMagic < _maxMagic)
            {
                CurrentMagic += _maxMagic * _modifyMagicCount;

                await Task.Delay(_regenerationIntervalMS);
            }

            CurrentMagic = Math.Clamp(CurrentMagic, 0, _maxMagic);

            _isRegening = false;
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace ProjectProvines.Core.System
{
    /// <summary>
    /// A class that implements the magic of an object.
    /// </summary>
    public class MagicSystem
    {
        public Single CurrentMagic { get; private set; }

        private readonly Single _maxMagic;
        private readonly Single _modifyMagicCount = 0.18f;

        private readonly Single _regenerationInterval = 0.6f;
        private readonly Single _cooldownBeforeRegeneration = 2.4f;
        private readonly Single _regenerationThreshold = 0.35f;

        private bool _isRegening;

        public MagicSystem(Single maxMagic)
        {
            _maxMagic = maxMagic;
            CurrentMagic = _maxMagic;
        }

        /// <summary>
        /// Changes the magic to a new specified amount.
        /// </summary>
        /// <param name="amount">The amount of new magic.</param>
        public void ModifyMagic(Single amount)
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
        private IEnumerator RegenerateMagic()
        {
            yield return new WaitForSeconds(_cooldownBeforeRegeneration);
            _isRegening = true;

            while (CurrentMagic < _maxMagic)
            {
                CurrentMagic += _maxMagic * _modifyMagicCount;

                yield return new WaitForSeconds(_regenerationInterval);
            }

            CurrentMagic = Math.Clamp(CurrentMagic, 0, _maxMagic);

            _isRegening = false;
        }
    }
}
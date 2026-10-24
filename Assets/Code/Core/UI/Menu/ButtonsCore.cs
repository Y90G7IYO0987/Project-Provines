using UnityEngine;

namespace ProjectProvines.Core.UI
{
    /// <summary>
    /// A class that works with button indices.
    /// </summary>
    public class ButtonsCore
    {
        private readonly int _maxIndex;
        private readonly int _startPosition;

        private int _currentIndex;

        /// <summary>
        /// Indexes initialization.
        /// </summary>
        public ButtonsCore(int maxIndex, int startIndex)
        {
            _maxIndex = maxIndex;
            _startPosition = startIndex;
            _currentIndex = _startPosition;
        }

        /// <summary>
        /// Replaces the index with a new one. Prevents it from going out of bounds.
        /// </summary>
        public void UpdateIndex(int index)
        {
            if (index > _maxIndex)
            {
                Debug.LogWarning("Buttons index out of range!");
                return;
            }

            _currentIndex = index;
        }

        /// <returns>Current button index.</returns>
        public int GetCurrentIndex() => _currentIndex;
    }
}
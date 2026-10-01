using System;

namespace ProjectProvines.Core.UI
{
    public class ButtonsCore
    {
        private readonly int _maxIndex;
        private readonly int _startPosition;

        private int _currentIndex;

        /// <summary>
        /// Конструктор с назначением индексов для кнопок.
        /// </summary>
        /// <param name="maxIndex">Максимальный индекс.</param>
        /// <param name="startPosition">Начальный индекс</param>
        public ButtonsCore(int maxIndex, int startPosition)
        {
            _maxIndex = maxIndex;
            _startPosition = startPosition;
            _currentIndex = _startPosition;
        }

        /// <summary>
        /// Сменяет индекс, новым-переданным. Не дает индексу выйти за пределы.
        /// </summary>
        /// <param name="index">Индекс для обновления.</param>
        public void UpdateIndex(int index)
        {
            _currentIndex = index;
        }

        public int GetCurrentIndex() => _currentIndex;
    }
}
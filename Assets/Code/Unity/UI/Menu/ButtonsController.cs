using ProjectProvines.Core.UI;
using ProjectProvines.Unity.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    public class ButtonsController : MonoBehaviour
    {
        [SerializeField] private InterfaceInput interfaceInput;

        [SerializeField] private List<Button> buttons;
        [SerializeField] private List<GameObject> screens;
        // Список картинок для нижнего слайда(эффект нажатой кнопки)
        [SerializeField] private List<Image> slideBars;

        private ButtonsCore _buttonsCore;

        private Dictionary<int, GameObject> indexToScreen = new Dictionary<int, GameObject>();
        // Получение по индексу картинки эффекта нажатой кнопки.
        private Dictionary<int, Image> indexToSlide = new Dictionary<int, Image>();

        // Текущее открытое окно.
        private GameObject _openedScreen;
        // Текущий, активный эффект нижнего слайда.
        private Image _activeSlide;

        private const int StartPosition = 0;

        /// <summary>
        /// Ставит основные словари, создает новый компонент с чистой логикой.
        /// </summary>
        private void Awake()
        {
            _buttonsCore = new ButtonsCore(buttons.Count, StartPosition);

            if (buttons.Count > 0)
                for (int i = 0; i < buttons.Count; i++)
                {
                    var button = buttons[i];

                    button.onClick.AddListener(() => OnButtonClick(button));
                }

            if (screens.Count > 0)
                for (int i = 0; i < screens.Count; i++)
                    indexToScreen[i] = screens[i];

            if (slideBars.Count > 0)
                for (int i = 0; i < slideBars.Count; i++)
                    indexToSlide[i] = slideBars[i];
        }

        /// <summary>
        /// Подписка на события.
        /// </summary>
        private void Start()
        {
            interfaceInput.OnMenuSwitchers += OnMenuSwitchers;
        }

        /// <summary>
        /// Возвращает индекс по кнопке.
        /// </summary>
        /// <param name="button">Кнопка меню.</param>
        /// <returns>Индекс кнопки</returns>
        private int GetIndexByButton(Button button)
        {
            if (buttons.Contains(button))
                return buttons.IndexOf(button);

            return StartPosition;
        }

        /// <summary>
        /// Обрабатывает основную логику нажатия на кнопку.
        /// </summary>
        /// <param name="clickedButton">Кнопка для получения индекса.</param>
        private void OnButtonClick(Button clickedButton)
        {
            int index = GetIndexByButton(clickedButton);
            _buttonsCore.UpdateIndex(index);
            OpenWindow(index);
            CreateSlideBar(index);
        }

        /// <summary>
        /// Открывает окно меню (инвентарь, карта...).
        /// </summary>
        /// <param name="index">Индекс кнопки</param>
        private void OpenWindow(int index)
        {
            if (indexToScreen.TryGetValue(index, out GameObject screen))
            {
                if (_openedScreen != null)
                    _openedScreen.SetActive(false);

                screen.SetActive(true);
                _openedScreen = screen;
            }
        }

        /// <summary>
        /// Создает эффект нажатой кнопки.
        /// </summary>
        /// <param name="index">Индекс кнопки</param>
        private void CreateSlideBar(int index)
        {
            if (indexToSlide.TryGetValue(index, out Image slideImage))
            {
                if (_activeSlide != null)
                    _activeSlide.enabled = false;

                _activeSlide = slideImage;
                slideImage.enabled = true;
            }
        }

        /// <summary>   
        /// Выполняет перемещение с помощью клавиш (вправо,влево) по кнопкам.
        /// </summary>
        /// <param name="moveIndex">(-1;1) Индекс для перемещения.</param>
        private void OnMenuSwitchers(int moveIndex)
        {
            int startIndex = _buttonsCore.GetCurrentIndex();
            int next = startIndex + moveIndex;

            if (next < StartPosition || next >= buttons.Count) return;
            OnButtonClick(buttons[next]);
        }

        private void OnDestroy()
        {
            interfaceInput.OnMenuSwitchers -= OnMenuSwitchers;
        }
    }
}
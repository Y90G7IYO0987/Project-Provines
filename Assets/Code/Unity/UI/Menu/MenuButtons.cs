using ProjectProvines.Core.UI;
using ProjectProvines.Unity.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    /// <summary>
    /// The main navigation class for menu buttons.
    /// </summary>
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] private InterfaceInput interfaceInput;

        [SerializeField] private List<Button> buttons;
        [SerializeField] private List<GameObject> screens;

        /// <summary>
        /// List of button press effects.
        /// </summary>
        [SerializeField] private List<Image> slideBars;

        private ButtonsCore _buttonsCore;

        private Dictionary<int, GameObject> indexToScreen = new Dictionary<int, GameObject>();

        /// <summary>
        /// A dictionary of images with the effect of a pressed button.
        /// </summary>
        private Dictionary<int, Image> indexToSlide = new Dictionary<int, Image>();

        /// <summary>
        /// The current open window.
        /// </summary>
        private GameObject _openedScreen;

        /// <summary>
        /// Current, active effect.
        /// </summary>
        private Image _activeSlide;

        private const int StartPosition = 0;

        /// <summary>
        /// Initializes the main dictionaries and creates a core with pure logic.
        /// </summary>
        private void Awake()
        {
            _buttonsCore = new ButtonsCore(buttons.Count, StartPosition);

            if (buttons.Count == 0 || screens.Count == 0 || slideBars.Count == 0)
                return;

            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];

                button.onClick.AddListener(() => OnButtonClick(button));
            }

            for (int i = 0; i < screens.Count; i++)
                indexToScreen[i] = screens[i];

            for (int i = 0; i < slideBars.Count; i++)
                indexToSlide[i] = slideBars[i];
        }

        private void Start()
        {
            interfaceInput.OnMenuSwitchers += OnMenuSwitchers;
        }

        /// <returns>Button index</returns>
        private int GetIndexByButton(Button button)
        {
            if (buttons.Contains(button))
                return buttons.IndexOf(button);

            return StartPosition;
        }

        /// <summary>
        /// Handles the main logic of button clicks.
        /// </summary>
        private void OnButtonClick(Button clickedButton)
        {
            int index = GetIndexByButton(clickedButton);
            _buttonsCore.UpdateIndex(index);
            OpenWindow(index);
            CreateSlideBar(index);
        }

        /// <summary>
        /// Opens the menu window.
        /// </summary>
        /// <param name="index">Button index.</param>
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
        /// Creates the effect of a pressed button.
        /// </summary>
        /// <param name="index">Button index.</param>
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
        /// Handles movement across buttons.
        /// </summary>
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
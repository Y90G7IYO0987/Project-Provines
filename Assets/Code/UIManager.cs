using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public struct MenuInformation
{
    public GameInput GameInput;
    public Image[] MenuActiveLines;
    public GameObject MenuUI;
    public List<GameObject> MenuWindows;
    public List<RectTransform> OtherUI;
}

public class MenuButtonsCharacteristic
{
    public int CurrentPosition { get; private set; }
    public int MaxPosition { get; private set; }

    public void SetIndex(int index) => CurrentPosition = index;

    public MenuButtonsCharacteristic(int maxPosition)
    {
        MaxPosition = maxPosition;
    }

    public void MoveOn()
    {
        int plusAmount = CurrentPosition < MaxPosition ? 1 : 0;
        CurrentPosition += plusAmount;
    }

    public void MoveOff()
    {
        int minusAmount = CurrentPosition > 0 ? 1 : 0;
        CurrentPosition -= minusAmount;
    }
}

public class MenuManager : MonoBehaviour
{
    public int MaxMenuWindows { get; private set; }

    public MenuButtonsCharacteristic MenuButtonsMovement { get; private set; }
    public Image[] MenuActiveLines { get; private set; }
    public GameObject MenuUI;

    private float _lastOpenedMenu;
    private float _openedMenuCooldown = 0.33f;

    private List<GameObject> _menuWindowsList;
    private Dictionary<string, GameObject> _menuWindows;
    private List<RectTransform> _otherUI;

    private GameObject _openedWindow;
    private MenuButtonLines _menuButtonLines;

    private const string DefaultWindowName = "Map";    

    public void Initialize(MenuInformation menuInfo)
    {
        MenuUI = menuInfo.MenuUI;
        MenuActiveLines = menuInfo.MenuActiveLines;

        _menuWindowsList = menuInfo.MenuWindows;
        _menuWindows = new Dictionary<string, GameObject>();

        MaxMenuWindows = _menuWindowsList.Count;
        MenuButtonsMovement = new MenuButtonsCharacteristic(MaxMenuWindows);
        _otherUI = menuInfo.OtherUI;

        foreach (GameObject menuWindow in _menuWindowsList)
        {
            if (menuWindow == null) continue;

            _menuWindows.Add(menuWindow.name, menuWindow);
        }

        _menuButtonLines = GetComponent<MenuButtonLines>();

        _menuButtonLines.Initialize(menuInfo.GameInput, MenuActiveLines, MenuUI);
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (Time.time - _lastOpenedMenu < _openedMenuCooldown)
        {
            Debug.Log("Menu cooldown!");
            return;
        }

        _lastOpenedMenu = Time.time;

        foreach (RectTransform ui in _otherUI)
        {
            if (ui == null) continue;

            ui.gameObject.SetActive(MenuUI.activeInHierarchy);
        }

        MenuUI.SetActive(!MenuUI.activeInHierarchy);                      

        if (MenuUI.activeInHierarchy) OpenDefaultWindow();
    }

    public void OpenMenuWindow(string menuWindowName)
    {
        if (_openedWindow != null && _openedWindow.name != menuWindowName)
        {
            _openedWindow.SetActive(false);
        }

        if (_menuWindows.TryGetValue(menuWindowName, out GameObject menu))
        {
            _openedWindow = menu;
            menu.SetActive(true);
        }
    }

    public void OpenDefaultWindow()
    {
        if (_openedWindow != null)
        {
            _openedWindow.SetActive(false);
            _openedWindow = null;
        }

        if (_menuWindows.TryGetValue(DefaultWindowName, out GameObject menuWindow))
        {
            _openedWindow = menuWindow;
            menuWindow.SetActive(true);
        }

        _menuButtonLines.OpenButtonLine(_menuButtonLines.DefaultActiveLine);
    }
}

public class MenuButtonLines : MonoBehaviour
{
    public Image DefaultActiveLine { get; private set; }

    private InputActions _inputActions;

    private Image[] _menuActiveLines;
    private Image _activeLine;
    private GameObject _menuUI;

    private Coroutine _delayCoroutine;

    private MenuButtonsCharacteristic _navigation;
    private MenuManager _menuManager;
    private Dictionary<int, string> _correctMenuWindowNames;
    private int _maxMenuWindows;

    private void Awake()
    {        
        _correctMenuWindowNames = new Dictionary<int, string>()
        {
            { 0, "Map" },
            { 1, "Inventory" },
            { 2, "Craft" },
            { 3, "Skills" },
            { 4, "Diary" },
        };
    }

    private void Start()
    {
        DefaultActiveLine = _menuActiveLines[0];
        _menuManager = GetComponent<MenuManager>();

        _maxMenuWindows = _menuManager.MaxMenuWindows;
        _navigation = new MenuButtonsCharacteristic(_maxMenuWindows);

        _inputActions.Player.MenuMovement.started += StartMovingRoutine;
        _inputActions.Player.MenuMovement.performed += Moving;
        _inputActions.Player.MenuMovement.canceled += StopMovingRoutine;
        _inputActions.Player.MenuInteraction.performed += _menuManager.OpenMenu;
    }

    public void Initialize(GameInput gameInput, Image[] menuActiveLines, GameObject menuUI)
    {
        _inputActions = gameInput.InputActions;
        _menuActiveLines = menuActiveLines;
        _menuUI = menuUI;
    }

    public void SetNewButtonLine(int index)
    {
        if (_navigation.CurrentPosition != index) _navigation.SetIndex(index);

        if (_activeLine != null) _activeLine.enabled = false;

        _activeLine = _menuActiveLines[index];

        _activeLine.enabled = true;

        if (_correctMenuWindowNames.TryGetValue(index, out string frameName))
        {
            _menuManager.OpenMenuWindow(frameName);
        }
    }

    public void OpenButtonLine(Image buttonLineImage)
    {
        if (_activeLine != null)
        {
            _activeLine.enabled = false;
        }

        _activeLine = buttonLineImage;
        _activeLine.enabled = true;
    }

    private IEnumerator StartMoving(InputAction.CallbackContext context)
    {
        if (!_menuUI.activeInHierarchy)
        {
            Debug.Log("Main menu not enabled!");
            yield break;
        }

        yield return new WaitForSeconds(0.8f);

        int currentPosition = _navigation.CurrentPosition;
        string controlName = context.control.name;

        if (_navigation.CurrentPosition >= _maxMenuWindows && controlName == "rightArrow")
        {
            Debug.Log("Max out value, cannot continue!");
            yield break;
        }

        if (_navigation.CurrentPosition == 0 && controlName == "leftArrow")
        {
            Debug.Log("Min out value, cannot continue!");
            yield break;
        }

        while (currentPosition < _maxMenuWindows && currentPosition != 0)
        {
            Moving(context);
            currentPosition = _navigation.CurrentPosition;

            yield return new WaitForSeconds(0.4f);
        }
    }

    private void StartMovingRoutine(InputAction.CallbackContext context)
    {
        if (_delayCoroutine != null) StopMovingRoutine(context);

        _delayCoroutine = StartCoroutine(StartMoving(context));
    }

    private void StopMovingRoutine(InputAction.CallbackContext context)
    {
        if (_delayCoroutine != null)
        {
            StopCoroutine(_delayCoroutine);
            _delayCoroutine = null;
        }
    }

    private void Moving(InputAction.CallbackContext context)
    {
        if (!_menuUI.activeInHierarchy)
        {
            Debug.Log("Main menu not enabled!");
            return;
        }

        string pressedKey = context.control.name;

        if (pressedKey == "rightArrow") _navigation.MoveOn();
        else if (pressedKey == "leftArrow") _navigation.MoveOff();

        int currentIndex = _navigation.CurrentPosition;

        SetNewButtonLine(currentIndex);

        Debug.Log($"Opened menu - {_activeLine} her enabled - {_activeLine.enabled}");

        Debug.Log($"Current position - {_navigation.CurrentPosition} of | {_navigation.MaxPosition}.");
    }

    private void OnDestroy()
    {
        _inputActions.Player.MenuMovement.started -= StartMovingRoutine;
        _inputActions.Player.MenuMovement.performed -= Moving;
        _inputActions.Player.MenuMovement.canceled -= StopMovingRoutine;
        _inputActions.Player.MenuInteraction.performed -= _menuManager.OpenMenu;
    }
}

public class MenuButtonsManagement : MonoBehaviour
{
    private float _lastClick;
    private float _clickCooldown = 0.2f;
    private MenuButtonLines _menuButtonLines;
    private List<Button> _buttonsList;

    public void Initialize(MenuButtonLines menuButtonLines, List<Button> buttonsList)
    {
        _menuButtonLines = menuButtonLines;
        _buttonsList = buttonsList;
    }

    public void OnButtonClick(Button clickedButton)
    {
        if (Time.time - _lastClick < _clickCooldown)
        {
            Debug.Log("Cant clicking!!!!!!!!!");
            return;
        }

        _lastClick = Time.time;

        int buttonIndex = _buttonsList.IndexOf(clickedButton);

        _menuButtonLines.SetNewButtonLine(buttonIndex);

        Debug.Log($"You clicked on {clickedButton.name}.");
    }
}

public class ArrowsManagement : MonoBehaviour
{
    // Here wrote arrows animations
}

public class UIManager : MonoBehaviour
{
    [Header("Main Stats")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float slidersUpdateSpeed = 3.0f;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject menuUI;

    [Header("Game Input")]
    [SerializeField] private GameInput gameInput;

    [Header("Arrow Images")]
    [SerializeField] private Image rightArrow;
    [SerializeField] private Image leftArrow;

    [Header("Lists")]
    [SerializeField] private Image[] menuActiveLines;
    [SerializeField] private List<Button> menuButtonsList;
    [SerializeField] private List<GameObject> menuWindowsList;
    [SerializeField] private List<RectTransform> otherUI;

    private MenuManager _menuManager;
    private MenuButtonLines _menuButtons;
    private MenuButtonsManagement _menuButtonsManagement;
    private MenuInformation _menuInformation;

    private void Awake()
    {
        _menuManager = gameObject.AddComponent<MenuManager>();
        _menuButtons = gameObject.AddComponent<MenuButtonLines>();
        _menuButtonsManagement = gameObject.AddComponent<MenuButtonsManagement>();

        _menuInformation = new MenuInformation()
        {
            GameInput = gameInput,
            MenuActiveLines = menuActiveLines,
            MenuUI = menuUI,
            MenuWindows = menuWindowsList,
            OtherUI = otherUI
        };

        // adding click to opening menu buttons
        foreach (Button button in menuButtonsList)
        {
            if (button == null) continue;

            button.onClick.AddListener(() => _menuButtonsManagement.OnButtonClick(button));
        }
    }

    private void Start()
    {
        _menuManager.Initialize(_menuInformation);
        _menuButtonsManagement.Initialize(_menuButtons, menuButtonsList);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public struct SlidersData
{
    public Slider HealthSlider;
    public Slider StaminaSlider;
    public PlayerData PlayerData;
    public float SlidersUpdateSpeed;
}

public class BarSliders : MonoBehaviour
{
    private Slider _healthSlider;
    private Slider _staminaSlider;
    private PlayerData _playerData;

    private float _slidersUpdateSpeed;
    private bool _isHealthUpdated;
    private bool _isStaminaUpdated;

    private void Update()
    {
        _isHealthUpdated = _healthSlider.value != _playerData.CurrentHealth ? true : false;
        _isStaminaUpdated = _staminaSlider.value != _playerData.CurrentStamina ? true : false;

        if (_isHealthUpdated) _healthSlider.value = Mathf.MoveTowards(_healthSlider.value, _playerData.CurrentHealth, Time.deltaTime * _slidersUpdateSpeed);

        if (_isStaminaUpdated) _staminaSlider.value = Mathf.MoveTowards(_staminaSlider.value, _playerData.CurrentStamina, Time.deltaTime * _slidersUpdateSpeed);

        if (_isHealthUpdated || _isStaminaUpdated) Debug.Log($"Some sliders updated: \n\n Current slider values:\n\n Health Slider value -> {_healthSlider.value},\n\n Stamina Slider value -> {_staminaSlider.value}.");
    }

    public void Initialize(SlidersData slidersData)
    {
        _healthSlider = slidersData.HealthSlider;
        _staminaSlider = slidersData.StaminaSlider;
        _playerData = slidersData.PlayerData;
        _slidersUpdateSpeed = slidersData.SlidersUpdateSpeed;
    }
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

public class MenuButtonLines : MonoBehaviour
{
    private InputActions _inputActions;
    private MenuButtonsCharacteristic _menuButtonsMovement;
    private Image[] _menuActiveLines;
    private Image _currentOpenedMenu;
    private Coroutine _delayCoroutine;
    private GameObject _mainMenuUI;
    private float _lastOpenedMenu;
    private float _openedMenuCooldown = 0.5f;

    private const int MaxPosition = 4;

    private void Awake()
    {
        _menuButtonsMovement = new MenuButtonsCharacteristic(MaxPosition);
    }

    private void Start()
    {
        _inputActions.Player.MenuMovement.started += StartMovingRoutine;
        _inputActions.Player.MenuMovement.performed += Moving;
        _inputActions.Player.MenuMovement.canceled += StopMovingRoutine;
        _inputActions.Player.MenuInteraction.performed += OpenMenuWindow;
    }

    public void Initialize(GameInput gameInput, Image[] menuActiveLines, GameObject mainMenu)
    {
        _inputActions = gameInput.InputActions;
        _menuActiveLines = menuActiveLines;
        _mainMenuUI = mainMenu;
    }

    public void SetNewMenu(int index)
    {
        if (_menuButtonsMovement.CurrentPosition != index) _menuButtonsMovement.SetIndex(index);

        if (_currentOpenedMenu != null) _currentOpenedMenu.enabled = false;

        if (index < _menuActiveLines.Length)
        {
            _currentOpenedMenu = _menuActiveLines[index];
        }

        if (_currentOpenedMenu != null) _currentOpenedMenu.enabled = true;
    }

    public void OpenMenuWindow(InputAction.CallbackContext context)
    {
        if (Time.time - _lastOpenedMenu < _openedMenuCooldown)
        {
            Debug.Log("Menu cooldown!");
            return;
        }

        _lastOpenedMenu = Time.time;

        _mainMenuUI.SetActive(!_mainMenuUI.activeInHierarchy);
    }
    private IEnumerator StartMoving(InputAction.CallbackContext context)
    {
        if (!_mainMenuUI.activeInHierarchy)
        {
            Debug.Log("Main menu not enabled!");
            yield break;
        }

        yield return new WaitForSeconds(0.8f);

        int currentPosition = _menuButtonsMovement.CurrentPosition;
        string controlName = context.control.name;

        if (_menuButtonsMovement.CurrentPosition >= MaxPosition && controlName == "rightArrow")
        {
            Debug.Log("Max out value, cannot continue!");
            yield break;
        }

        if (_menuButtonsMovement.CurrentPosition == 0 && controlName == "leftArrow")
        {
            Debug.Log("Min out value, cannot continue!");
            yield break;
        }

        while (currentPosition < MaxPosition && currentPosition != 0)
        {
            Moving(context);
            currentPosition = _menuButtonsMovement.CurrentPosition;

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
        if (!_mainMenuUI.activeInHierarchy)
        {
            Debug.Log("Main menu not enabled!");
            return;
        }

        string pressedKey = context.control.name;

        if (pressedKey == "rightArrow") _menuButtonsMovement.MoveOn();
        else if (pressedKey == "leftArrow") _menuButtonsMovement.MoveOff();

        int currentIndex = _menuButtonsMovement.CurrentPosition;

        SetNewMenu(currentIndex);

        Debug.Log($"Opened menu - {_currentOpenedMenu} her enabled - {_currentOpenedMenu.enabled}");

        Debug.Log($"Current position - {_menuButtonsMovement.CurrentPosition} of | {_menuButtonsMovement.MaxPosition}.");
    }

    private void OnDestroy()
    {
        _inputActions.Player.MenuMovement.started -= StartMovingRoutine;
        _inputActions.Player.MenuMovement.performed -= Moving;
        _inputActions.Player.MenuMovement.canceled -= StopMovingRoutine;
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

        _menuButtonLines.SetNewMenu(buttonIndex);

        Debug.Log($"You clicked on {clickedButton.name}.");
    }
}

public class ArrowsManagement : MonoBehaviour
{
    // Here wrote arrows animations
}

public class AnimationsConfigure : MonoBehaviour
{
    public readonly int RunningHash = Animator.StringToHash(Running);

    private Animator _animator;
    private PlayerData _playerData;

    private const string Running = "IsRunning";
    private bool _isRunning;

    private void Update()
    {
        _animator.SetBool(RunningHash, _isRunning);
    }

    public void Initialize(PlayerData playerData)
    {
        _playerData = playerData;
        _animator = playerData.Animator;
        _isRunning = _playerData.IsRunning;
    }
}

public class UIManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float slidersUpdateSpeed = 3.0f;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject mainUI;

    [Header("Game Input")]
    [SerializeField] private GameInput gameInput;

    [Header("Arrow Images")]
    [SerializeField] private Image rightArrow;
    [SerializeField] private Image leftArrow;

    [Header("Lists")]
    [SerializeField] private Image[] menuActiveLines;
    [SerializeField] private List<Button> menuButtonsList;

    private SlidersData _receiverData;
    private BarSliders _barSliders;
    private AnimationsConfigure _animationsConfigure;
    private MenuButtonLines _menuButtons;
    private MenuButtonsManagement _menuButtonsManagement;

    private void Awake()
    {
        _receiverData = new SlidersData
        {
            HealthSlider = healthSlider,
            StaminaSlider = staminaSlider,
            SlidersUpdateSpeed = slidersUpdateSpeed,
            PlayerData = playerData
        };

        _barSliders = gameObject.AddComponent<BarSliders>();
        _animationsConfigure = gameObject.AddComponent<AnimationsConfigure>();
        _menuButtons = gameObject.AddComponent<MenuButtonLines>();
        _menuButtonsManagement = gameObject.AddComponent<MenuButtonsManagement>();

        foreach (Button button in menuButtonsList)
        {
            if (button == null) continue;

            button.onClick.AddListener(() => _menuButtonsManagement.OnButtonClick(button));
        }
    }

    private void Start()
    {
        _barSliders.Initialize(_receiverData);
        _animationsConfigure.Initialize(playerData);
        _menuButtons.Initialize(gameInput, menuActiveLines, mainUI);
        _menuButtonsManagement.Initialize(_menuButtons, menuButtonsList);
    }
}

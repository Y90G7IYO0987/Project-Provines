using UnityEngine;

public class GameInput : MonoBehaviour
{
    public InputActions InputActions { get; private set; }

    [SerializeField] private PlayerData playerData;

    private Player _player;

    private void Awake()
    {
        InputActions = new InputActions();
        InputActions.Enable();

        _player = playerData.Prefab.GetComponent<Player>();
        Debug.Log($"Player GameObject - {_player}");
        _player.SetInputActions(InputActions);
    }
}

using UnityEngine;

public class GameInput : MonoBehaviour
{
    public InputActions InputActions { get; private set; }

    private void Awake()
    {
        InputActions = new InputActions();
        InputActions.Enable();
    }
}

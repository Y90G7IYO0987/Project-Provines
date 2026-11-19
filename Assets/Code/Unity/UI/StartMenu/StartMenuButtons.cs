using ProjectProvines.Core.Interfaces;
using ProjectProvines.Core.Models;
using ProjectProvines.Unity.Scenes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuButtons : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;

    private Dictionary<string, Action> _buttonsInteraction;

    private ISceneController _sceneController;

    private GameScene _gameScene;

    private void Awake()
    {
        _buttonsInteraction = new Dictionary<string, Action>()
        {
            { "Continue", () => ContinueButtonInteraction() },
            { "NewGame", () => NewGameButtonInteraction() },
            { "Load", () => LoadButtonInteraction() },
            { "Options", () => OptionsButtonInteraction() },
            { "ExitGame", () => ExitGameButtonInteraction() },
        };

        if (_buttons.Count > 0)
        {
            foreach (var button in _buttons)
            {
                if (button == null)
                    continue;

                if (_buttonsInteraction.TryGetValue(button.name, out Action func))
                {
                    String buttonName = button.name;

                    button.onClick.AddListener(() => func()); 

                    Debug.Log($"Successfuly works: {button.name}");
                }
            }
        }

        _sceneController = new SceneController();
        _gameScene = new GameScene();
    }

    private void ContinueButtonInteraction()
    {
        _sceneController.LoadNewScene(_gameScene.BuildIndex);
    }

    private void NewGameButtonInteraction()
    {

    }

    private void LoadButtonInteraction()
    {

    }

    private void OptionsButtonInteraction()
    {

    }

    private void ExitGameButtonInteraction()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

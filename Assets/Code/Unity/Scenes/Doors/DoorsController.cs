using ProjectProvines.Core.Interfaces;
using UnityEngine;

namespace ProjectProvines.Unity.Scenes
{
    /// <summary>
    /// Class for handling movements to other scenes.
    /// </summary>
    public class DoorsController : MonoBehaviour
    {
        private ISceneController _sceneController;

        private void Awake()
        {
            _sceneController = new SceneController();
        }

        private void OnTriggerEnter(Collider _)
        {
            _sceneController.LoadNewScene(gameObject.tag);
        }
    }
}
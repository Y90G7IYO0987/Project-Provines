using ProjectProvines.Core.Interfaces;
using UnityEngine.SceneManagement;

namespace ProjectProvines.Unity.Scenes
{
    public class SceneController : ISceneController
    {
        public void LoadNewScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadNewScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }
    }
}
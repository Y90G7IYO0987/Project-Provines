namespace ProjectProvines.Core.Interfaces
{
    public interface ISceneController
    {
        void LoadNewScene(string sceneName);
        void LoadNewScene(int buildIndex);
    }
}
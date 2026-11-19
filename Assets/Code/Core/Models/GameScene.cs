namespace ProjectProvines.Core.Models
{
    public sealed class GameScene
    {
        public string Name { get; set; } = "GameScene";
        public int BuildIndex { get; set; } = 1;
        public bool IsNewGame { get; set; }
    }
}
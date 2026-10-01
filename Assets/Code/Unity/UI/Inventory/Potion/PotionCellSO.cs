using UnityEngine;

namespace ProjectProvines.Unity.Scriptables
{
    public enum PotionType
    {
        Gold,
        Amethysts
    }

    [CreateAssetMenu(fileName = "New Potion Data", menuName = "Inventory/PotionData")]
    public class PotionCell : CellData
    {
        public float Multiplier;
        public float Duration;
        public PotionType PotionType;
    }
}
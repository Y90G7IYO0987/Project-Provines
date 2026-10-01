using UnityEngine;

namespace ProjectProvines.Unity.Scriptables
{
    public enum ArmorType
    {
        Helmet,
        Breastplate,
        Boots,
        Belt,
        Ring,
        Amulet
    }

    [CreateAssetMenu(fileName = "New Armor Data", menuName = "Inventory/ArmorData")]
    public class ArmorCell : CellData
    {
        public float ArmorPercent;
        public ArmorType ArmorType;
    }
}
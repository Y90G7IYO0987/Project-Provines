using UnityEngine;

namespace ProjectProvines.Unity.Scriptables
{    
    [CreateAssetMenu(fileName = "New Armor Data", menuName = "Inventory/ArmorData")]
    public class ArmorCell : CellData
    {
        public float ArmorPercent;
        public ArmorType ArmorType;
    }
}
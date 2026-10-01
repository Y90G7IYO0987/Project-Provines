using UnityEngine;

namespace ProjectProvines.Unity.Scriptables
{
    public enum WeaponCellType
    {
        Static,
        Dynamic
    }

    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Inventory/WeaponData")]
    public class WeaponCell : CellData
    {
        public float Damage;
        public WeaponCellType WeaponType;
    }
}

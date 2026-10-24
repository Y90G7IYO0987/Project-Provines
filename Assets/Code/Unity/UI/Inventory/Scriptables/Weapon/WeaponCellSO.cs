using UnityEngine;

namespace ProjectProvines.Unity.Scriptables
{   
    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Inventory/WeaponData")]
    public class WeaponCell : CellData
    {
        public float Damage;
        public WeaponCellType WeaponType;
    }
}

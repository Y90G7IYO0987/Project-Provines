using ProjectProvines.Unity.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    /// <summary>
    /// Collects all slot data together. Puts items into free slots.
    /// </summary>
    public class InventorySlots : MonoBehaviour
    {
        private SlotsDataManager _dataManager;

        private void Awake()
        {
            _dataManager = GetComponent<SlotsDataManager>();
        }

        /// <summary>
        /// Moving the weapon to the slot.
        /// </summary>
        /// <param name="itemData">Abstract class thing.</param>
        public void EquipItemInSlot(CellData itemData, string guid)
        {
            if (itemData.CellType == CellType.Weapon)
            {
                Image emptySlot = _dataManager.GetEmptyWeaponSlot(guid);

                if (emptySlot == null)
                    return;

                _dataManager.AddNewTool(guid, emptySlot, itemData.Icon);
            }
        }

        /// <summary>
        /// Removing a weapon from the slot.
        /// Erases the slot data.
        /// </summary>
        public void UnequipItemInSlot(string guid)
        {
            var filledSlot = _dataManager.GetSlotByGuid(guid);

            if (filledSlot == null)
                return;

            _dataManager.ResetData(filledSlot, guid);
        }
    }
}
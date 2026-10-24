using ProjectProvines.Core.UI;
using ProjectProvines.Unity.Input;
using UnityEngine;

namespace ProjectProvines.Unity.UI
{
    /// <summary>
    /// Processes the main input for the inventory menu.
    /// </summary>
    public class InventoryInteraction : MonoBehaviour
    {
        [SerializeField] private InterfaceInput interfaceInput;
        [SerializeField] private InventorySlots inventorySlots;

        private SelectedItemData _selectedItemData;

        private void Start()
        {
            _selectedItemData = GetComponent<InventoryController>().GetItemData();

            interfaceInput.OnCellEquipping += EquipItem;
            interfaceInput.OnCellStopHolding += StopHoldingCell;
        }

        /// <summary>
        /// Click handler.
        /// Manages the insertion of the item into the slot.
        /// </summary>
        private void EquipItem()
        {
            if (!gameObject.activeInHierarchy || _selectedItemData.ItemData == null)
                return;

            inventorySlots.EquipItemInSlot(_selectedItemData.ItemData, _selectedItemData.ItemGuid);
        }

        /// <summary>
        /// Click handler.
        /// Manages the removal of an item from the slot.
        /// </summary>
        private void StopHoldingCell()
        {
            if (!gameObject.activeInHierarchy)
                return;
            if (_selectedItemData.ItemData == null)
                return;

            inventorySlots.UnequipItemInSlot(_selectedItemData.ItemGuid);
        }

        private void OnDestroy()
        {
            interfaceInput.OnCellEquipping -= EquipItem;
            interfaceInput.OnCellStopHolding -= StopHoldingCell;
        }
    }
}
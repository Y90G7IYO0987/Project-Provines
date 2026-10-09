using ProjectProvines.Core.UI;
using ProjectProvines.Unity.Input;
using UnityEngine;

namespace ProjectProvines.Unity.UI
{
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

        // Обработчик нажатий.
        // Руководит надеванием предмета в слот.
        private void EquipItem()
        {
            if (!gameObject.activeInHierarchy || _selectedItemData.ItemData == null)
                return;

            inventorySlots.EquipItemInSlot(_selectedItemData.ItemData, _selectedItemData.ItemGuid);
        }

        // Обработчик нажатий.
        // Руководит снятием предмета из слота.
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
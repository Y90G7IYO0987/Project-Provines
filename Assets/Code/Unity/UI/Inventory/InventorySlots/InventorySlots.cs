using ProjectProvines.Unity.UI;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlots : MonoBehaviour
{
    public int EquippedItems { get; private set; }

    private SlotsDataManager _dataManager;

    private void Awake()
    {
        _dataManager = GetComponent<SlotsDataManager>();
    }

    /// <summary>
    /// Перемещение оружия в слот.
    /// </summary>
    /// <param name="itemData">Абстрактный класс информации о вещи.</param>
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

    // Снятие оружия из слота.
    // Ресет даты о слоте и guid в менеджере.
    public void UnequipItemInSlot(string guid)
    {
        var filledSlot = _dataManager.GetSlotByGuid(guid);

        if (filledSlot == null)
            return;

        _dataManager.ResetData(filledSlot, guid);
    }
}

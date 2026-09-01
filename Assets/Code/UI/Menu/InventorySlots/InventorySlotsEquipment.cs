using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotsEquipment : MonoBehaviour
{
    private Dictionary<RectTransform, InventoryCellData> _equippedSlotsData;

    public bool IsItemEquipped(InventoryCellData itemData) => _equippedSlotsData.ContainsValue(itemData);

    private void Awake()
    {
        _equippedSlotsData = new Dictionary<RectTransform, InventoryCellData>();
    }

    public void EquipItemInSlot(RectTransform slot, InventoryCellData itemData)
    {
        Image icon = slot.GetChild(0).GetComponent<Image>();
        icon.sprite = itemData.ItemImage;

        _equippedSlotsData.Add(slot, itemData);
    }

    public void UnequipItem(RectTransform slot)
    {
        bool canClean = _equippedSlotsData.ContainsKey(slot) ? true : false;
        if (canClean) _equippedSlotsData.Remove(slot);
    }
}

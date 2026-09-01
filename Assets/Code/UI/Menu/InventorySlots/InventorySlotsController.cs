using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotsController : MonoBehaviour
{
    [SerializeField] private List<RectTransform> hudSlots = new List<RectTransform>();

    private Dictionary<RectTransform, bool> _weaponSlots = new Dictionary<RectTransform, bool>();
    private Dictionary<InventoryCellData, RectTransform> _equippedWeaponRects;

    private InventorySlotsEquipment _inventorySlotsEquipment;

    private const string WeaponSlotTag = "WeaponSlot";

    private void Awake()
    {
        _inventorySlotsEquipment = GetComponent<InventorySlotsEquipment>();
        _equippedWeaponRects = new Dictionary<InventoryCellData, RectTransform>();

        foreach (RectTransform slot in hudSlots)
        {
            if (slot == null) continue;

            if (slot.CompareTag(WeaponSlotTag))
            {
                Debug.Log($"Found: {slot.gameObject.tag}");
                _weaponSlots.Add(slot, false);
            }
        }
    }

    public void OnEquipButtonClick(InventoryCellData itemData)
    {
        if (itemData.ItemType == InventoryCellType.Weapon)
        {
            RectTransform activeWeaponSlot = GetActiveWeaponSlot();
            if (activeWeaponSlot == null) return;
            if (_inventorySlotsEquipment.IsItemEquipped(itemData))
            {
                Debug.LogWarning("This item is already equipped!");
                return;
            }

            _weaponSlots[activeWeaponSlot] = true;
            _equippedWeaponRects.Add(itemData, activeWeaponSlot);

            _inventorySlotsEquipment.EquipItemInSlot(activeWeaponSlot, itemData);
        }
    }

    public void OnUnequipButtonClick(InventoryCellData itemData)
    {
        RectTransform slot;
        if (_equippedWeaponRects.TryGetValue(itemData, out slot))
        {
            Debug.Log($"Found this slot in dict - {slot}.");

            _inventorySlotsEquipment.UnequipItem(slot);
            _equippedWeaponRects.Remove(itemData);
        }            
    }

    // Checks any weapon slots is empty.
    private bool IsAnyWeaponSlotsEmpty()
    {
        bool result = false;

        foreach (KeyValuePair<RectTransform, bool> pair in _weaponSlots)
        {
            if (pair.Key && !pair.Value)
            {
                result = true;
                break;
            }
        }

        Debug.LogWarning($"Is any weapon slots is empty: {result}.");

        return result;
    }

    // Get first empty slot.
    private RectTransform GetActiveWeaponSlot()
    {
        bool isSlotsEmpty = IsAnyWeaponSlotsEmpty();
        if (!isSlotsEmpty) return null;

        RectTransform result = null;

        foreach (KeyValuePair<RectTransform, bool> pair in _weaponSlots)
        {
            if (pair.Key && !pair.Value)
            {
                result = pair.Key;
            }
        }

        Debug.Log($"Result - {result}");

        return result;
    }    
}

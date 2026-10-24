using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    /// <summary>
    /// The main guide for storing and equipping items in slots.
    /// </summary>
    public class SlotsDataManager : MonoBehaviour
    {
        [SerializeField] private List<Image> slots;

        /// <summary>
        /// The date of the assigned slot objects.
        /// </summary>
        private List<Image> _equippedItems = new List<Image>();

        /// <summary>
        /// A dictionary with a key in the form of a GUID and a slot value.
        /// </summary>
        private Dictionary<string, Image> _guidToItem = new Dictionary<string, Image>();

        /// <summary>
        /// Returns an empty, available slot.
        /// </summary>
        /// <returns>Empty available slot.</returns>
        public Image GetEmptyWeaponSlot(string guid)
        {
            Image emptySlot = null;

            foreach (var slot in slots)
            {
                if (slot == null || !slot.CompareTag("Weapon"))
                    continue;

                if (_equippedItems.Contains(slot))
                    continue;

                if (_guidToItem.ContainsKey(guid))
                    continue;

                emptySlot = slot;

                break;
            }

            return emptySlot;
        }

        /// <summary>
        /// Adds a new item to the slot.
        /// Updates the data in the collections.
        /// </summary>
        /// <param name="slot">Slot object.</param>
        /// <param name="icon">Item icon.</param>
        public void AddNewTool(string guid, Image slot, Sprite icon)
        {
            var slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
            slotIcon.sprite = icon;
            slotIcon.enabled = true;

            _equippedItems.Add(slot);
            _guidToItem.Add(guid, slot);
        }

        /// <summary>
        /// Returns the slot by GUID.
        /// </summary>
        public Image GetSlotByGuid(string guid)
        {
            if (_guidToItem.TryGetValue(guid, out Image result)) { }

            return result;
        }

        /// <summary>
        /// Cleans the data. Removes the item.
        /// </summary>
        public void ResetData(Image slot, string guid)
        {
            var icon = slot.transform.GetChild(0).GetComponent<Image>();
            icon.sprite = null;
            icon.enabled = false;

            if (_equippedItems.Contains(slot))
                _equippedItems.Remove(slot);

            if (_guidToItem.ContainsKey(guid))
                _guidToItem.Remove(guid);
        }
    }
}
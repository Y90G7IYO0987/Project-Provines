using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    public class SlotsDataManager : MonoBehaviour
    {
        [SerializeField] private List<Image> slots;

        // Дата о надетых экземплярах слотов.
        private List<Image> _equippedItems = new List<Image>();
        // Словарь с ключом в виде GUID, и значением слота.
        private Dictionary<string, Image> _guidToItem = new Dictionary<string, Image>();

        // Возвращает пустой, доступный слот.
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

        // Добавляет новый предмет в слот.
        // Обновляет данные в коллекциях.
        public void AddNewTool(string guid, Image slot, Sprite icon)
        {
            var slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
            slotIcon.sprite = icon;
            slotIcon.enabled = true;

            _equippedItems.Add(slot);
            _guidToItem.Add(guid, slot);
        }

        // Возвращает слот по данному guid.
        public Image GetSlotByGuid(string guid)
        {
            if (_guidToItem.TryGetValue(guid, out Image result)) { }

            return result;
        }

        // Чистит данные.
        // Снимает предмет.
        public void ResetData(Image slot, string guid)
        {
            var icon = slot.transform.GetChild(0).GetComponent<Image>();
            icon.sprite = null;
            icon.enabled = false;

            if (_equippedItems.Contains(slot))
                _equippedItems.Remove(slot);

            if (_guidToItem.ContainsKey(guid))
                _guidToItem.Remove(guid);

            Debug.Log("Successfuly removed all!");
        }
    }
}
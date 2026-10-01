using ProjectProvines.Core.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    public struct ActionsData
    {
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI NameText;
        public CellData ItemData;
        public SelectedItemData SelectedItemData;
        public string ItemGuid;
    }

    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private List<CellData> playerCells;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject itemsContainer;

        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemTitleText;

        private SelectedItemData _selectedItemData;

        private List<Image> _createdItems = new List<Image>();

        private void Awake()
        {
            _selectedItemData = new SelectedItemData();

            for (int i = 0; i < playerCells.Count; i++)
            {
                var item = playerCells[i];
                CreateNewItem(item);
            }
        }        

        public void CreateNewItem(CellData itemData)
        {
            var newItem = Instantiate(itemPrefab, itemsContainer.transform);
            newItem.name = itemData.Name;
            newItem.tag = itemData.CellType.ToString();

            var image = newItem.GetComponent<Image>();
            image.sprite = itemData.Icon;

            _createdItems.Add(image);

            string newGuid = CreateNewGuid();

            var data = new ActionsData
            {
                TitleText = itemTitleText,
                NameText = itemNameText,
                ItemData = itemData,
                SelectedItemData = _selectedItemData,
                ItemGuid = newGuid
            };

            var itemActions = image.GetComponent<ItemActions>();            
            itemActions.Initialize(data);
        }

        /// <summary>
        /// Создает уникальный идентификатор.
        /// </summary>
        /// <returns>Новый гид.</returns>
        private string CreateNewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <returns>Возвращает созданные ячейки инвентаря в виде листа.</returns>
        public List<Image> GetInventoryItems()
        {
            Debug.Log($"Count: {_createdItems.Count}");
            return _createdItems;
        }
    }    
}

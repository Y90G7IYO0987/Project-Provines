using ProjectProvines.Core.UI;
using ProjectProvines.Unity.Scriptables;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{    
    /// <summary>
    /// Combines the main components to ensure the proper operation of the inventory.
    /// </summary>
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private List<CellData> playerCells;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject itemsContainer;

        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemTitleText;

        private SelectedItemData _selectedItemData;

        private List<Image> _createdItems = new List<Image>();

        public SelectedItemData GetItemData() => _selectedItemData;

        public List<Image> GetInventoryItems() => _createdItems;

        private void Awake()
        {
            _selectedItemData = new SelectedItemData();

            for (int i = 0; i < playerCells.Count; i++)
            {
                var item = playerCells[i];
                CreateNewItem(item);
            }
        }

        /// <summary>
        /// Creates an object.
        /// Sets and records the main parameters.
        /// It connects the required class.
        /// </summary>
        /// <param name="itemData">Abstract class of the subject.</param>
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
        /// Creates a unique key.
        /// </summary>
        /// <returns>Unique key.</returns>
        private string CreateNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
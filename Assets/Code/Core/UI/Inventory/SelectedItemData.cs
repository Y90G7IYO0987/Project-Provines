using UnityEngine.UI;

namespace ProjectProvines.Core.UI
{
    public class SelectedItemData
    {
        // Экземпляр выбранного предмета.
        public Image SelectedItem { get; private set; }
        // SO Информация о предмете.
        public CellData ItemData { get; private set; }
        public string ItemGuid { get; private set; }

        // Обновляет параметры.
        // Записывает характеристики для нового предмета.
        public void SetNewItem(Image newItem, CellData itemData, string itemGuid)
        {
            SelectedItem = newItem;
            ItemData = itemData;
            ItemGuid = itemGuid;
        }
    }
}
using UnityEngine.UI;

namespace ProjectProvines.Core.UI
{
    public class SelectedItemData
    {
        public Image SelectedItem { get; private set; }
        public CellData ItemData { get; private set; }
        public string ItemGuid { get; private set; }

        public void SetNewItem(Image newItem, CellData itemData, string itemGuid)
        {
            SelectedItem = newItem;
            ItemData = itemData;
            ItemGuid = itemGuid;
        }
    }
}
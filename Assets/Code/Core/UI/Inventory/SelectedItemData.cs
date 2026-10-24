using ProjectProvines.Unity.Scriptables;
using UnityEngine.UI;

namespace ProjectProvines.Core.UI
{
    /// <summary>
    /// Contains information about the selected item from the inventory.
    /// </summary>
    public class SelectedItemData
    {
        /// <summary>
        /// The object of the selected item.
        /// </summary>
        public Image SelectedItem { get; private set; }

        /// <summary>
        /// SO Information about the subject.
        /// </summary>
        public CellData ItemData { get; private set; }
        public string ItemGuid { get; private set; }

        /// <summary>
        /// Updates the parameters.
        /// Records the characteristics for the new item.
        /// </summary>
        /// <param name="newItem">An instance of the item.</param>
        /// <param name="itemData">Complete information about the subject.</param>
        /// <param name="itemGuid">Identification key.</param>
        public void SetNewItem(Image newItem, CellData itemData, string itemGuid)
        {
            SelectedItem = newItem;
            ItemData = itemData;
            ItemGuid = itemGuid;
        }
    }
}
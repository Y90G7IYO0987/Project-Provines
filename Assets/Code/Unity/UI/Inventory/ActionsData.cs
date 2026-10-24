using ProjectProvines.Core.UI;
using ProjectProvines.Unity.Scriptables;
using TMPro;

namespace ProjectProvines.Unity.UI
{
    /// <summary>
    /// A data structure that stores information about an inventory cell.
    /// </summary>
    public struct ActionsData
    {
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI NameText;
        public CellData ItemData;
        public SelectedItemData SelectedItemData;
        public string ItemGuid;
    }
}
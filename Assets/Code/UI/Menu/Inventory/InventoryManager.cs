using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemActions : MonoBehaviour, IPointerClickHandler
{
    public InventoryCellData ItemData { get; private set; }
    private TextMeshProUGUI _mainStats;

    public void SetItemData(InventoryCellData data) => ItemData = data;
    public void SetMainStats(TextMeshProUGUI mainStats) => _mainStats = mainStats;
    private string GetItemStats() => ItemData?.GetStats() ?? "No Data";

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"You clicked on {gameObject.name}.");
        Debug.Log($"Tries to get item data... {gameObject.name} Data: {GetItemStats()}");
        _mainStats.text = GetItemStats();
    }
}

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> inventoryCells;
    [SerializeField] private GameObject inventoryCellPrefab;
    [SerializeField] private InventoryCellData[] inventoryCellsData;
    [SerializeField] private GameObject cells;
    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private TextMeshProUGUI mainStats;

    private void Start()
    {
        for (int i = 0; i < 28; i++)
        {
            var randomDataSO = inventoryCellsData[Random.Range(0, inventoryCellsData.Length)];

            CreateNewCell(randomDataSO);
        }
    }

    public void CreateNewCell(InventoryCellData cellData)
    {
        var newInventoryCell = Instantiate(inventoryCellPrefab, cells.transform);
        newInventoryCell.name = cellData.ItemName;

        var itemActions = newInventoryCell.AddComponent<InventoryItemActions>();
        itemActions.SetItemData(cellData);
        itemActions.SetMainStats(mainStats);

        var cellIcon = newInventoryCell.GetComponentInChildren<Image>();
        var cellNameText = newInventoryCell.GetComponentInChildren<TextMeshProUGUI>();

        cellIcon.sprite = cellData.ItemImage;
        cellNameText.text = cellData.ItemName;

        var background = newInventoryCell.AddComponent<Image>();
        background.sprite = defaultBackground;

        Debug.Log($"Created new cell -> {newInventoryCell.name}.");
    }
}

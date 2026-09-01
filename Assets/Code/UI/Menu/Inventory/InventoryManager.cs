using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour
{
    public InventoryCellData SelectedItem { get; private set; }

    public void SetSelectedItem(InventoryCellData itemData) => SelectedItem = itemData;
}

public class InventoryItemActions : MonoBehaviour, IPointerClickHandler
{
    private InventoryCellData _itemData;
    private TextMeshProUGUI _mainStats;
    private InventoryItems _inventoryItems;

    private string GetItemStats() => _itemData?.GetStats() ?? "No Data";

    public void Initialize(InventoryCellData itemData, TextMeshProUGUI mainStats, InventoryItems items)
    {
        Debug.Log($"Created GUILD -> {itemData.Guid}");
        _itemData = itemData;
        _mainStats = mainStats;
        _inventoryItems = items;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_inventoryItems.SelectedItem != null && _inventoryItems.SelectedItem.Guid == _itemData.Guid)
        {
            Debug.Log($"Selected-Guild - {_inventoryItems.SelectedItem.Guid} Current Item-Guild - {_itemData.Guid}.");
            Debug.LogWarning($"This item is already selected - {_itemData.ItemName}!");
            return;
        }

        _inventoryItems.SetSelectedItem(_itemData);

        Debug.Log($"You clicked on {gameObject.name}.");
        Debug.Log($"Tries to get item data... {gameObject.name} Data: {GetItemStats()}");
        _mainStats.text = GetItemStats();
    }
}

public class InventoryManager : MonoBehaviour
{
    public Button EquipButton { get; private set; }
    public Button UpgradeButton { get; private set; }
    public InventoryItems InventoryItems { get; private set; }

    [SerializeField] private GameObject inventoryCellPrefab;
    [SerializeField] private InventoryCellData[] inventoryCellsData;
    [SerializeField] private GameObject cells;
    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private TextMeshProUGUI mainStats;
    [SerializeField] private Button[] actionButtons = new Button[1];
    [SerializeField] private InventorySlotsController inventorySlotsController;

    private void Awake()
    {
        EquipButton = actionButtons[0];
        UpgradeButton = actionButtons[1];

        InventoryItems = gameObject.AddComponent<InventoryItems>();

        EquipButton.onClick.AddListener(() => EquipButtonLogic(InventoryItems.SelectedItem));
        UpgradeButton.onClick.AddListener(() => UpgradeButtonLogic(InventoryItems.SelectedItem));
    }

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            CreateNewCell(inventoryCellsData[0]);
        }
    }

    public void CreateNewCell(InventoryCellData originalCellData)
    {
        InventoryCellData cellData = Instantiate(originalCellData);

        GenerateNewGuid(cellData);
        Debug.Log($"GUILD -> {cellData.Guid}");

        var newInventoryCell = Instantiate(inventoryCellPrefab, cells.transform);
        newInventoryCell.name = cellData.ItemName;

        var itemActions = newInventoryCell.AddComponent<InventoryItemActions>();
        itemActions.Initialize(cellData, mainStats, InventoryItems);

        var cellIcon = newInventoryCell.GetComponentInChildren<Image>();
        var cellNameText = newInventoryCell.GetComponentInChildren<TextMeshProUGUI>();

        cellIcon.sprite = cellData.ItemImage;
        cellNameText.text = cellData.ItemName;

        var background = newInventoryCell.AddComponent<Image>();
        background.sprite = defaultBackground;

        Debug.Log($"Created new cell -> {newInventoryCell.name}.");
    }

    private void GenerateNewGuid(InventoryCellData cellData)
    {
        cellData.Guid = Guid.NewGuid().ToString();
    }

    private void EquipButtonLogic(InventoryCellData itemData)
    {
        if (itemData == null)
        {
            Debug.LogWarning("Select any item!");
            return;
        }

        inventorySlotsController.OnEquipButtonClick(itemData);
    }

    private void UpgradeButtonLogic(InventoryCellData itemData)
    {
        //inventorySlotsController.OnUpgradeButtonClick(itemData);
    }
}

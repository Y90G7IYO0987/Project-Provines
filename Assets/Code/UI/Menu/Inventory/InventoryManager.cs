using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] List<GameObject> inventoryCells;
    [SerializeField] GameObject inventoryCellPrefab;
    [SerializeField] InventoryWeapon spearWeaponData;
    [SerializeField] GameObject cells;
    [SerializeField] private Sprite defaultBackground;

    private void Start()
    {
        for (int i = 0; i < 28; i++)
        {
            CreateNewCell(spearWeaponData);
        }
    }

    public void CreateNewCell(InventoryCellData cellData)
    {
        var newInventoryCell = Instantiate(inventoryCellPrefab, cells.transform);
        newInventoryCell.name = cellData.ItemName;

        var cellIcon = newInventoryCell.GetComponentInChildren<Image>();
        var cellNameText = newInventoryCell.GetComponentInChildren<TextMeshProUGUI>();

        cellIcon.sprite = cellData.ItemImage;
        cellNameText.text = cellData.ItemName;

        var background = newInventoryCell.AddComponent<Image>();
        background.sprite = defaultBackground;

        Debug.Log($"Created new cell -> {newInventoryCell.name}.");
    }
}

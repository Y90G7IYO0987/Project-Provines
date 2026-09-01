using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Inventory/Armor")]
public class InventoryArmor : InventoryCellData
{
    [Space]
    [Header("Armor Settings /")]
    public float DefenceAmount;
    public GameObject ArmorPrefab;

    public override string GetStats()
    {
        return $"Name: {ItemName}\n" +
               $"Armor Properties /\n\n" +
               $"Armor: {DefenceAmount}\n";
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Inventory/Buff")]
public class InventoryBuff : InventoryCellData
{
    [Space]
    [Header("Buff Settings /")]
    public float Duration;
    public float BuffMultiplier;
    public BuffType BuffType;

    public override string GetStats()
    {
        return $"Name: {ItemName}\n" +
               $"Buff Properties /\n\n" +
               $"Duration: {Duration}\n" +
               $"Multiplier: {BuffMultiplier}\n" +
               $"Type: {BuffType}\n";
    }
}
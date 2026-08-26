using UnityEngine;

public enum InventoryCellType
{
    Weapon,
    Armor,
    Buff
}

public enum WeaponType
{
    Static, // fists
    Dynamic // katana, sword
}

public enum BuffType
{
    Coins,
    Damage,
    Armor
}

public abstract class InventoryCellData : ScriptableObject
{
    [Header("Main Settings /")]
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemImage;
    public InventoryCellType ItemType;

    public virtual string GetStats()
    {
        return $"Name: {ItemName}\n" +
               $"Description: {ItemDescription}\n" +
               $"Type: {ItemType}\n";
    }
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Inventory/Weapon")]
public class InventoryWeapon : InventoryCellData
{
    [Space]
    [Header("Weapon Settings /")]
    public float Damage;
    public float AttackCooldown;
    public float AttackDistance;
    public GameObject WeaponPrefab;
    public WeaponType WeaponType;

    public override string GetStats()
    {
        return $"Name: {ItemName}\n" +
               $"Description: {ItemDescription}\n" +
               $"Sword Properties /\n\n" +
               $"Damage: {Damage}\n" +
               $"Speed: {AttackCooldown}\n" +
               $"Range: {AttackDistance} \n" +
               $"Type: {WeaponType}\n";
    }
}

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
               $"Description: {ItemDescription}\n" +
               $"Type: {ItemType}\n" +
               $"Armor Properties /\n\n" +
               $"Armor: {DefenceAmount}\n";
    }
}

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
               $"Description: {ItemDescription}\n" +
               $"Type: {ItemType}\n" +
               $"Buff Properties /\n\n" +
               $"Duration: {Duration}\n" +
               $"Multiplier: {BuffMultiplier}\n" +
               $"Type: {BuffType}\n";
    }
}
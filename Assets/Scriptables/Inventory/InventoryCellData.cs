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
    public Sprite ItemImage;
    public string Guid;
    public InventoryCellType ItemType;

    public virtual string GetStats()
    {
        return $"Name: {ItemName}\n" +
               $"Type: {ItemType}\n";
    }
}
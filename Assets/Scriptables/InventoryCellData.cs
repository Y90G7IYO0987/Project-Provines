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
}

[CreateAssetMenu(fileName = "NewArmor", menuName = "Inventory/Armor")]
public class InventoryArmor : InventoryCellData
{
    [Space]
    [Header("Armor Settings /")]
    public float DefenceAmount;
    public GameObject ArmorPrefab;
}

[CreateAssetMenu(fileName = "NewBuff", menuName = "Inventory/Buff")]
public class InventoryBuff : InventoryCellData
{
    [Space]
    [Header("Buff Settings /")]
    public float Duration;
    public float BuffMultiplier;
    public BuffType BuffType;
}
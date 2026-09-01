using UnityEngine;

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
               $"Sword Properties /\n\n" +
               $"Damage: {Damage}\n" +
               $"Speed: {AttackCooldown}\n" +
               $"Range: {AttackDistance} \n";
    }
}
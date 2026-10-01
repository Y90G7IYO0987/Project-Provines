using UnityEngine;

public enum CellType
{
    Weapon,
    Potion,
    Armor
}

public abstract class CellData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public GameObject Prefab;
    public CellType CellType;
}
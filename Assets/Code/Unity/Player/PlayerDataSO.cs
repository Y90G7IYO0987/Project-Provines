using ProjectProvines.Core.Utils;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float MaxHealth;
    public float MaxMagic;
    public float MaxStamina;

    public GameObject Prefab;
    public PlayerUtils PlayerUtils;
}
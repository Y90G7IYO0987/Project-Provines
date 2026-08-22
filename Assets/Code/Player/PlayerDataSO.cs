using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create Player Data")]
public class PlayerData : ScriptableObject
{
    public float MaxHealth;
    public float MaxStamina;
    public float CurrentHealth;
    public float CurrentStamina;
    public float MaxDamage;
    public bool IsRunning;
    public GameObject Prefab;
    public Animator Animator;
}

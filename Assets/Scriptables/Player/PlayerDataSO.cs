using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create Player Data")]
public class PlayerData : ScriptableObject
{
    public float MaxHealth;
    public float MaxStamina;    
    public float MaxDamage;
    public GameObject Prefab;
    public Animator Animator;
}

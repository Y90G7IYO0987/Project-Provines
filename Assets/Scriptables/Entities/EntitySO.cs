using UnityEngine;

[CreateAssetMenu(fileName = "NewEntity", menuName = "Entity/MainEntityData")]
public class EntityData : ScriptableObject
{
    public string EntityName;
    public string EntityDescription;
    public float MaxHealth;
    public float MaxStamina;
    public float Damage;
    public bool IsFriendlyEntity;
    public bool IsChasingEntity;
    public EntityMovingStates StartEntityState;
}
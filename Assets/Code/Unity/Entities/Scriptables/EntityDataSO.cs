using ProjectProvines.Core.Entities;
using UnityEngine;

namespace ProjectProvines.Unity.Scriptables
{
    public abstract class EntityData : ScriptableObject
    {
        public string Name;
        public string Description;

        public float MaxHealth;

        public EntityTypeSpecification EntityTypeSpecification;
        public WeaponType WeaponType;

        public GameObject Prefab;
        public Sprite Icon;
    }
}

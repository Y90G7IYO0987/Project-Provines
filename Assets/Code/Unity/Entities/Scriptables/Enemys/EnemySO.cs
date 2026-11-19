using UnityEngine;

namespace ProjectProvines.Unity.Scriptables
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Entities/Enemy")]
    public class EnemyData : EntityData
    {
        public int LevelRequirement;
        public float BaseXPReward;

        public float Damage;
        public float AttackAnimationsCount = 2f;

        public EnemyRarity Rarity;
    }
}
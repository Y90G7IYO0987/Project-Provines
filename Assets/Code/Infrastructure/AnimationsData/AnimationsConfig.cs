using UnityEngine;

namespace ProjectProvines.Core.Infrastructure
{
    /// <summary>
    /// The main parameters of hashes and constants.
    /// </summary>
    public static class AnimationsConfig
    {
        /// <summary>
        /// Constant fields of the player’s attacks.
        /// </summary>
        public static class PlayerAttacks
        {
            public const float StartAttackCount = 1f;
            public const float MaxAttacksCount = 3f;
        }

        /// <summary>
        /// The main fields of the player’s hashes.
        /// </summary>
        public static class Player
        {
            public static readonly int XPosition = Animator.StringToHash("XPosition");
            public static readonly int ZPosition = Animator.StringToHash("ZPosition");
            public static readonly int AttackCount = Animator.StringToHash("AttackCount");
            public static readonly int GetPlayerHit = Animator.StringToHash("GetPlayerHit");
            public static readonly int Attack = Animator.StringToHash("Attack");
            public static readonly int Jump = Animator.StringToHash("Jump");
            public static readonly int IsRunning = Animator.StringToHash("IsRunning");
        }

        /// <summary>
        /// The main fields of entity hashes.
        /// </summary>
        public static class Entity
        {
            public static readonly int XPosition = Animator.StringToHash("XPosition");
            public static readonly int ZPosition = Animator.StringToHash("ZPosition");
            public static readonly int CurrentAttack = Animator.StringToHash("CurrentAttack");            
        }

        /// <summary>
        /// Fields of entity hashes for animations.
        /// </summary>
        public static class EntityAnims
        {
            public static readonly int Attack = Animator.StringToHash("Attack");
            public static readonly int GetHitted = Animator.StringToHash("GetHitted");
            public static readonly int IsRunning = Animator.StringToHash("IsRunning");
            public static readonly int IsDie = Animator.StringToHash("IsDie");
        }

        /// <summary>
        /// The main constants of the entity attack.
        /// </summary>
        public static class EntityAttacks
        {
            public const float StartAttacksCount = 1f;
            public const float MaxAttacksCount = 2f;
        }
    }
}
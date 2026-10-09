using UnityEngine;

namespace ProjectProvines.Core.Infrastructure
{
    public static class AnimationsConfig
    {
        public static class Attacks
        {
            public const float StartAttackCount = 0f;
            public const float MaxAttacksCount = 2f;
        }

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

        public static class EntityAnims
        {
            public static readonly int XPosition = Animator.StringToHash("XPosition");
            public static readonly int ZPosition = Animator.StringToHash("ZPosition");
            public static readonly int CurrentAttack = Animator.StringToHash("CurrentAttack");
            public static readonly int Attack = Animator.StringToHash("Attack");
            public static readonly int GetHitted = Animator.StringToHash("GetHitted");
            public static readonly int IsRunning = Animator.StringToHash("IsRunning");
            public static readonly int IsDie = Animator.StringToHash("IsDie");
        }

        public static class EntityAttacks
        {
            public const float StartAttacksCount = 1f;
            public const float MaxAttacksCount = 2f;
        }
    }
}
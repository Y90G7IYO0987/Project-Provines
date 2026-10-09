using UnityEngine;

namespace ProjectProvines.Core.Infrastructure
{
    public static class EntityStatesHash
    {
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Movement = Animator.StringToHash("Movement");
        public static readonly int Running = Animator.StringToHash("Running");
        public static readonly int Chasing = Animator.StringToHash("Chasing");
        public static readonly int Attacking = Animator.StringToHash("Attacking");
        public static readonly int GetHitted = Animator.StringToHash("GetHitted");
        public static readonly int Die = Animator.StringToHash("Die");
    }
}
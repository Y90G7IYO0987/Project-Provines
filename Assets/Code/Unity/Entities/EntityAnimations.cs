using ProjectProvines.Core.Infrastructure;
using System.Collections;
using UnityEngine;

namespace ProjectProvines.Unity.Entities
{
    /// <summary>
    /// Processes entity animations.
    /// </summary>
    public class EntityAnimations : MonoBehaviour
    {
        public bool IsNowAttacking => _canPlayAttack;

        // Current movement along the x‑axis.
        private float _xAxis => _entityController.XPosition;
        private float _zAxis => _entityController.ZPosition;

        private float _currentAttack = StartAttacksCount;
        private float _maxAttacksCount = AnimationsConfig.EntityAttacks.MaxAttacksCount;

        private bool _canPlayAttack = true;

        private EntityController _entityController;

        private Animator _animator;

        private const float StartAttacksCount = AnimationsConfig.EntityAttacks.StartAttacksCount;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _entityController = GetComponent<EntityController>();
        }

        private void Start()
        {
            _entityController.OnEntityAttacking += SwitchAttacking;
        }

        private void Update()
        {
            SwitchWalking();
        }

        /// <summary>
        /// Processes movement animations.
        /// </summary>
        private void SwitchWalking()
        {
            _animator.SetFloat(AnimationsConfig.Entity.XPosition, _xAxis);
            _animator.SetFloat(AnimationsConfig.Entity.ZPosition, _zAxis);
        }

        /// <summary>
        /// Manages the attack state.
        /// Calculates the attack, the duration of the animation, and applies a delay.
        /// </summary>
        private void SwitchAttacking()
        {
            if (!_canPlayAttack)
                return;

            _animator.SetTrigger(AnimationsConfig.EntityAnims.Attack);

            if (_currentAttack >= _maxAttacksCount)
            {
                _currentAttack = 1f;
                _animator.SetFloat(AnimationsConfig.Entity.CurrentAttack, _currentAttack);
                StartCoroutine(CalculateAttackDelay());
                return;
            }

            _currentAttack++;
            _animator.SetFloat(AnimationsConfig.Entity.CurrentAttack, _currentAttack);
            StartCoroutine(CalculateAttackDelay());
        }

        /// <summary>
        /// Calculates the cooldown for the current attack.
        /// And adds a cooldown to the current attack.
        /// </summary>
        /// <returns>Animation duration.</returns>
        private IEnumerator CalculateAttackDelay()
        {
            _canPlayAttack = false;

            yield return new WaitForEndOfFrame();

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            float duration = stateInfo.length / stateInfo.speed;

            yield return new WaitForSeconds(duration);

            _canPlayAttack = true;
        }

        private void OnDestroy()
        {
            _entityController.OnEntityAttacking -= SwitchAttacking;
        }
    }

}
using ProjectProvines.Unity.Player;
using System.Collections;
using UnityEngine;

namespace ProjectProvines.Unity.Animations
{
    public class PlayerAnimations : MonoBehaviour
    {
        private Animator _animator;

        private PlayerPhysics _physics;

        private float _xAxis;
        private float _zAxis;

        private float _currentAttack = DefaultAttack;

        private bool _isAttacking;
        private bool _isRunning => _physics.IsRunning;

        private const float MaxAttacks = AnimationsConfig.Attacks.MaxAttacksCount;
        private const float DefaultAttack = AnimationsConfig.Attacks.StartAttackCount;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _physics = GetComponent<PlayerPhysics>();
        }

        private void Update()
        {
            _xAxis = _physics.XAxis;
            _zAxis = _physics.ZAxis;

            SetWalkingStates();
            TryRun();
        }

        public void TryJump()
        {
            _animator.SetTrigger(AnimationsConfig.Player.Jump);
        }       

        public void TryAttack()
        {
            if (_isAttacking)
                return;
            _animator.SetTrigger(AnimationsConfig.Player.Attack);
            CalculateAttack();
            StartCoroutine(AttackCooldownRoutine());
        }

        private void TryRun()
        {
            _animator.SetBool(AnimationsConfig.Player.IsRunning, _isRunning);
        }

        private void SetWalkingStates()
        {
            _animator.SetFloat(AnimationsConfig.Player.XPosition, _xAxis);
            _animator.SetFloat(AnimationsConfig.Player.ZPosition, _zAxis);
        }        

        private void CalculateAttack()
        {
            if (_currentAttack >= MaxAttacks)
                _currentAttack = DefaultAttack;
            _currentAttack += 1f;
            _animator.SetFloat(AnimationsConfig.Player.AttackCount, _currentAttack);
        }

        private float CalculateAttackCooldown()
        {
            _animator.Update(0f);

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            return stateInfo.length / stateInfo.speed;
        }

        private IEnumerator AttackCooldownRoutine()
        {
            _isAttacking = true;
            yield return new WaitForSeconds(CalculateAttackCooldown());
            _isAttacking = false;
        }
    }
}
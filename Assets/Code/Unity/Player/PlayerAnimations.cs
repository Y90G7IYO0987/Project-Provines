using ProjectProvines.Core.Infrastructure;
using ProjectProvines.Unity.Player;
using ProjectProvines.Unity.Scriptables;
using ProjectProvines.Unity.Weapon;
using System.Collections;
using UnityEngine;

namespace ProjectProvines.Unity.Animations
{
    /// <summary>
    /// Processes the player’s animations.
    /// </summary>
    [RequireComponent(typeof(PlayerData))]
    public class PlayerAnimations : MonoBehaviour
    {
        public bool IsPlayerAttacking => _isAttacking;

        [SerializeField] private PlayerData playerData;        

        private float _xAxis;
        private float _zAxis;

        private float _currentAttack = DefaultAttack;

        private bool _isAttacking;
        private bool _isRunning => _physics.IsRunning;

        private Animator _animator;

        private PlayerPhysics _physics;
        private WeaponController _weaponController;

        private const float MaxAttacks = AnimationsConfig.PlayerAttacks.MaxAttacksCount;
        private const float DefaultAttack = AnimationsConfig.PlayerAttacks.StartAttackCount;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _physics = GetComponent<PlayerPhysics>();
        }

        private void Start()
        {
            _weaponController = playerData.WeaponController;

            _weaponController.OnPlayerAttacking += TryAttack;
        }

        /// <summary>
        /// Provides motion animations.
        /// </summary>
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

        /// <summary>
        /// Creates an animation of the current required attack.
        /// </summary>
        public void TryAttack()
        {
            if (_isAttacking || _physics.IsRunning)
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

        /// <summary>
        /// Changes the current attack to a new one.
        /// </summary>
        private void CalculateAttack()
        {
            _animator.SetFloat(AnimationsConfig.Player.AttackCount, _currentAttack);
            Debug.Log($"Current attack: {_currentAttack}");

            _currentAttack++;

            if (_currentAttack > MaxAttacks)
                _currentAttack = DefaultAttack;
        }

        /// <summary>
        /// Calculates the delay between attacks.
        /// </summary>
        /// <returns>The delay between attacks</returns>
        private float CalculateAttackCooldown()
        {
            _animator.Update(0f);

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            return stateInfo.length / stateInfo.speed;
        }

        /// <summary>
        /// Adds a delay to the current attack.
        /// </summary>
        private IEnumerator AttackCooldownRoutine()
        {
            _isAttacking = true;
            yield return new WaitForSeconds(CalculateAttackCooldown());
            _isAttacking = false;
        }

        private void OnDestroy()
        {
            _weaponController.OnPlayerAttacking -= TryAttack;
        }
    }
}
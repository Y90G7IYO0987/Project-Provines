using ProjectProvines.Core.Infrastructure;
using System.Collections;
using UnityEngine;

namespace ProjectProvines.Unity.Entities
{
    public class EntityAnimations : MonoBehaviour
    {
        private Animator _animator;

        // Текущее перемещение по оси x.
        private float _xAxis => _entityController.XPosition;
        private float _zAxis => _entityController.ZPosition;

        private float _currentAttack = StartAttacksCount;
        private float _maxAttacksCount = AnimationsConfig.EntityAttacks.MaxAttacksCount;

        private EntityController _entityController;

        private bool _canPlayAttack = true;

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

        // Работа с анимациями перемещения.
        private void SwitchWalking()
        {
            _animator.SetFloat(AnimationsConfig.EntityAnims.XPosition, _xAxis);
            _animator.SetFloat(AnimationsConfig.EntityAnims.ZPosition, _zAxis);
        }

        /// <summary>
        /// Управляет самим состоянием атаки.
        /// Считает атаку, длительность анимации, выполняет задержку.
        /// </summary>
        private void SwitchAttacking()
        {
            if (!_canPlayAttack)
                return;

            _animator.SetTrigger(AnimationsConfig.EntityAnims.Attack);

            if (_currentAttack >= _maxAttacksCount)
            {
                _currentAttack = 1f;
                _animator.SetFloat(AnimationsConfig.EntityAnims.CurrentAttack, _currentAttack);
                StartCoroutine(CalculateAttackDelay());
                return;
            }

            _currentAttack++;
            _animator.SetFloat(AnimationsConfig.EntityAnims.CurrentAttack, _currentAttack);
            StartCoroutine(CalculateAttackDelay());
        }

        /// <summary>
        /// Получает информацию о длительности следующей анимации.
        /// </summary>
        /// <returns>Длительность анимации.</returns>
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
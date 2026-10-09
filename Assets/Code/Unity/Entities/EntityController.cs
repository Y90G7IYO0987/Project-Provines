using ProjectProvines.Core.Entities;
using ProjectProvines.Core.Infrastructure;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectProvines.Unity.Entities
{
    public class EntityController : MonoBehaviour
    {
        public event Action OnEntityAttacking;

        // Перемещение (X).
        public float XPosition { get; private set; }
        public float ZPosition { get; private set; }

        [SerializeField] private PlayerData playerData;

        [SerializeField] private float chasingDistance = 5f;
        [SerializeField] private float attackingDistance = 1.8f;
        [SerializeField] private float inputCooldown = 0.24f;

        private int _currentState => _entityCore.EntityState;

        private int _defaultState = EntityStatesHash.Idle;

        // Основное ядро логики.
        private EntityCore _entityCore;

        private GameObject _playerPrefab;
        private NavMeshAgent _agent;

        private bool _actuallyInput;

        private void Awake()
        {
            _entityCore = new EntityCore();

            _agent = GetComponent<NavMeshAgent>();

            _entityCore.ChangeEntityState(EntityStatesHash.Idle);
        }

        private void Start()
        {
            _playerPrefab = playerData.Prefab;

            _agent.stoppingDistance = attackingDistance;
        }

        private void Update()
        {
            ChooseState();
            ChangeEntityState();
            SetVelocityLengths();
        }

        /// <summary>
        /// Проверяет и выбирает состояние для сущности.
        /// </summary>
        private void ChooseState()
        {
            switch (_currentState)
            {
                case var h when h == EntityStatesHash.Idle:
                    break;
                case var h when h == EntityStatesHash.Movement:
                    break;
                case var h when h == EntityStatesHash.Running:
                    break;
                case var h when h == EntityStatesHash.Chasing:
                    ChasingPlayer();
                    break;
                case var h when h == EntityStatesHash.Attacking:
                    EntityAttacking();
                    break;
                case var h when h == EntityStatesHash.GetHitted:
                    break;
                case var h when h == EntityStatesHash.Die:
                    break;
            }
        }

        /// <summary>
        /// Изменяет текущее состояние сущности на новое определенное.
        /// </summary>
        private void ChangeEntityState()
        {
            int newState = _defaultState;

            float distanceToPlayer = _entityCore.CalculateDistance(transform.position, 
                _playerPrefab.transform.position);

            if (distanceToPlayer <= attackingDistance)
                newState = EntityStatesHash.Attacking;

            if (distanceToPlayer <= chasingDistance && distanceToPlayer > attackingDistance)
                newState = EntityStatesHash.Chasing;

            _entityCore.ChangeEntityState(newState);
        }

        /// <summary>
        /// Рандомная выборка точки и движение к ней.
        /// </summary>
        private void Move()
        {
            SetVelocityLengths();
        }

        /// <summary>
        /// Следование сущности за игроком.
        /// </summary>
        private void ChasingPlayer()
        {
            SetVelocityLengths();

            _agent.isStopped = false;

            _agent.destination = _playerPrefab.transform.position;
        }

        private void EntityAttacking()
        {
            _agent.isStopped = true;
            
            if (!_actuallyInput)
                StartCoroutine(InputRoutine());
        }

        // Задает путь/перемещение сущности.
        private void SetVelocityLengths()
        {
            Vector3 velocityVector = _agent.velocity.normalized;
            XPosition = velocityVector.x;
            ZPosition = velocityVector.z;
        }

        private IEnumerator InputRoutine()
        {
            _actuallyInput = true;

            OnEntityAttacking?.Invoke();

            yield return new WaitForSeconds(inputCooldown);

            _actuallyInput = false;
        }
    }
}
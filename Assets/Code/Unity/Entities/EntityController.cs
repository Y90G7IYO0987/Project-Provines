using ProjectProvines.Core.Entities;
using ProjectProvines.Core.Infrastructure;
using ProjectProvines.Core.Player;
using ProjectProvines.Unity.Scriptables;
using ProjectProvines.Unity.UI;
using ProjectProvines.Unity.Weapon;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectProvines.Unity.Entities
{
    /// <summary>
    /// Describes the behavior of the entity.
    /// </summary>
    public class EntityController : MonoBehaviour
    {
        public event Action OnEntityAttacking;

        // Movement (X).
        public float XPosition { get; private set; }
        public float ZPosition { get; private set; }

        [SerializeField] private BarSliders barSliders;

        [SerializeField] private PlayerData playerData;

        [SerializeField] private float chasingDistance = 5f;
        [SerializeField] private float attackingDistance = 1.8f;

        private int _currentState => _entityCore.EntityState;

        private int _defaultState = EntityStatesHash.Idle;

        private bool _canAttacking => _entityAnimations.IsNowAttacking;

        // The core of logic.
        private EntityCore _entityCore;
        private EntityAnimations _entityAnimations;

        private PlayerProperties _playerProperties;

        private GameObject _playerPrefab;
        private NavMeshAgent _agent;

        /// <summary>
        /// Initializes the main components of the entity.
        /// </summary>
        private void Awake()
        {
            _entityCore = new EntityCore();
            _entityAnimations = GetComponent<EntityAnimations>();

            _agent = GetComponent<NavMeshAgent>();

            _entityCore.ChangeEntityState(EntityStatesHash.Idle);
        }

        private void Start()
        {
            _playerPrefab = playerData.Prefab;
            _playerProperties = playerData.PlayerUtils.GetPlayerProperties();

            _agent.stoppingDistance = attackingDistance;
        }

        /// <summary>
        /// Changes the state of the entity.
        /// </summary>
        private void Update()
        {
            ChooseState();
            ChangeEntityState();
            SetVelocityLengths();
        }

        /// <summary>
        /// Depending on the current parameters, it selects the state for the entity.
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
        /// Changes the current state of the entity to a new, defined state.
        /// </summary>
        private void ChangeEntityState()
        {
            int newState = _defaultState;

            float distanceToPlayer = _entityCore.CalculateDistance(transform.position,
                _playerPrefab.transform.position);
            
            if (distanceToPlayer <= attackingDistance && _playerProperties.CurrentHealth > 0f)
                newState = EntityStatesHash.Attacking;

            if (distanceToPlayer <= chasingDistance && distanceToPlayer > attackingDistance && _playerProperties.CurrentHealth > 0f)
                newState = EntityStatesHash.Chasing;

            _entityCore.ChangeEntityState(newState);
        }

        /// <summary>
        /// Processes the arbitrary movement of an entity.
        /// </summary>
        private void Move()
        {
            SetVelocityLengths();
        }

        /// <summary>
        /// Handles the entity’s follow‑up to the player.
        /// </summary>
        private void ChasingPlayer()
        {
            SetVelocityLengths();

            _agent.isStopped = false;

            _agent.destination = _playerPrefab.transform.position;
        }

        /// <summary>
        /// Processes the entity attack.
        /// </summary>
        private void EntityAttacking()
        {
            _agent.isStopped = true;

            if (_canAttacking)
            {
                OnEntityAttacking?.Invoke();
                barSliders.TakeDamage(WeaponController.Instance.EquippedWeapon.Damage);
            }
        }

        // Sets the entity’s movement vector.
        private void SetVelocityLengths()
        {
            Vector3 velocityVector = _agent.velocity.normalized;
            XPosition = velocityVector.x;
            ZPosition = velocityVector.z;
        }
    }
}
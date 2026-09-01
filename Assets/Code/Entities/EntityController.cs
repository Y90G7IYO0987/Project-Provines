using UnityEngine;
using UnityEngine.AI;

public enum EntityMovingStates
{
    Idle,
    Movement,
    Chasing,
    Attacking
}

public class EntityController : MonoBehaviour
{
    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int MovementHash = Animator.StringToHash("Movement");
    private readonly int ChasingHash = Animator.StringToHash("Chasing");
    private readonly int AttackingHash = Animator.StringToHash("Attacking");

    [Header("Main Stats")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private EntityData entityData;
    [SerializeField] private float chasingDistance = 10.0f;
    [SerializeField] private float attackDistance = 2.0f;
    [SerializeField] private float movementRadius = 15f;
    [SerializeField] private float maxMovingAngle = 45f;
    [SerializeField] private float stayingTime = 5.0f;
    [SerializeField] private float targetReachDistance = 1f;
    [SerializeField] private float differenceStaminaTime = 1f;

    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private EntityVisual _entityVisual;

    private bool _isFriendly;
    private bool _isChasing;

    private string _currentState;
    private string _defaultState;
    private bool _isStaying;
    private float _lastStaminaChangesTime;

    public void SetStayingState(bool isEntityStaying) => _isStaying = isEntityStaying;
    public void ResetAgentPath() => _agent.ResetPath();
    private float CalculateDistance(Vector3 startPoint, Vector3 targetPoint) => Vector3.Distance(startPoint, targetPoint);

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _entityVisual = GetComponent<EntityVisual>();

        _currentState = entityData.StartEntityState.ToString();
        _defaultState = "Movement";

        _isFriendly = entityData.IsFriendlyEntity;
        _isChasing = entityData.IsChasingEntity;

        Debug.Log($"Start entity state - {_currentState}");
    }

    private void Start()
    {
        _playerTransform = playerData.Prefab.transform;
    }

    private void Update()
    {
        EntityStateActions();
    }

    public void SwitchEntityStates()
    {
        float distanceToPlayer = CalculateDistance(_playerTransform.position, transform.position);

        bool isChasing = distanceToPlayer < chasingDistance && distanceToPlayer > attackDistance ? true : false;
        bool isAttacking = distanceToPlayer < attackDistance ? true : false;

        string newState = _defaultState;

        if (isChasing) newState = "Chasing";

        if (isAttacking) newState = "Attacking";

        if (_isStaying && newState == _defaultState) newState = "Idle";

        _currentState = newState;
    }

    private void EntityStateActions()
    {
        int incomingHash = Animator.StringToHash(_currentState);

        switch (incomingHash)
        {
            case int when incomingHash == IdleHash:
                // мб допишу здесь поворот сущности к игроку.
                break;
            case int when incomingHash == MovementHash:
                if (Time.time - _lastStaminaChangesTime > differenceStaminaTime)
                {
                    _entityVisual.ChangeStamina();
                    _lastStaminaChangesTime = Time.time;
                }

                float currentStamina = _entityVisual.GetCurrentStamina();

                if (currentStamina > 0f) EntityMovement();

                SwitchEntityStates();
                break;
            case int when incomingHash == ChasingHash:
                Debug.Log("Now - Chasing state!");
                EntityChasing();
                SwitchEntityStates();
                break;
            case int when incomingHash == AttackingHash:
                Debug.Log("Now - Attacking state!");
                EntityAttacking();
                SwitchEntityStates();
                break;
        }
    }

    private void EntityChasing()
    {
        if (!_isChasing) return;

        Debug.Log("Chasing to player!");
        _agent.SetDestination(_playerTransform.position);
    }

    private void EntityAttacking()
    {
        if (_isFriendly) return;

        Debug.Log("Attacking player!");
        _agent.ResetPath();
    }

    private void EntityMovement()
    {
        if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
        {
            Vector3 target = GetMovementPosition(transform.position, movementRadius, maxMovingAngle);
            _agent.SetDestination(target);
        }
    }

    private Vector3 GetMovementPosition(Vector3 center, float radius, float maxAngle)
    {
        float randomAngle = Random.Range(-maxAngle, maxAngle);

        Vector3 direction = Quaternion.AngleAxis(randomAngle, Vector3.up) * transform.forward;

        Vector3 targetPosition = center + direction * radius;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 3f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return targetPosition;
    }
}

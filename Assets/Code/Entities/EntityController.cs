using System.Collections;
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
    [SerializeField] public float MaxEntityStamina = 100.0f;

    [Header("Main Stats")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float chasingDistance = 10.0f;
    [SerializeField] private float attackDistance = 2.0f;
    [SerializeField] private float movementRadius = 15f;
    [SerializeField] private float maxMovingAngle = 45f;
    [SerializeField] private float stayingTime = 5.0f;
    [SerializeField] private float targetReachDistance = 1f;
    [SerializeField] private float differenceStaminaTime = 1f;
    [SerializeField] private float changingStaminaCount = 5f;
    [SerializeField] private float regeningStaminaAmount = 8.0f;
    [SerializeField] private EntityMovingStates startEntityState;
    [SerializeField] private bool isFriendly = true;
    [SerializeField] private bool isChasing = false;

    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private string _currentState;
    private string _defaultState;
    private bool _isStaying;
    private float _lastStaminaChangesTime;
    private float _currentEntityStamina;
    private bool _isStaminaRegening;

    private float CalculateDistance(Vector3 startPoint, Vector3 targetPoint) => Vector3.Distance(startPoint, targetPoint);

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _currentState = startEntityState.ToString();
        _defaultState = EntityMovingStates.Movement.ToString();
        _currentEntityStamina = MaxEntityStamina;

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

    private void EntityStateActions()
    {
        switch (_currentState)
        {
            case "Idle":
                Debug.LogWarning("Idle condition");
                // мб допишу здесь поворот сущности к игроку.
                break;
            case "Movement":
                if (Time.time - _lastStaminaChangesTime > differenceStaminaTime)
                {
                    ChangeStamina(changingStaminaCount);
                    _lastStaminaChangesTime = Time.time;
                }

                if (_currentEntityStamina > 0f) EntityMovement();

                SwitchEntityStates();
                break;
            case "Chasing":
                Debug.Log("Now - Chasing state!");
                EntityChasing();
                SwitchEntityStates();
                break;
            case "Attacking":
                Debug.Log("Now - Attacking state!");
                EntityAttacking();
                SwitchEntityStates();
                break;
        }
    }

    private void SwitchEntityStates()
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

    private void ChangeStamina(float changeCount)
    {
        if (_isStaminaRegening) return;

        if (_currentEntityStamina == 0f)
        {
            StartCoroutine(RegenStaminaRoutine());

            return;
        }

        _currentEntityStamina -= changeCount;
        _currentEntityStamina = Mathf.Clamp(_currentEntityStamina, 0f, MaxEntityStamina);        
    }

    private void EntityChasing()
    {
        if (!isChasing) return;

        Debug.Log("Chasing to player!");
        _agent.SetDestination(_playerTransform.position);
    }

    private void EntityAttacking()
    {
        if (isFriendly) return;

        Debug.Log("Attacking player!");
        _agent.ResetPath();
    }

    private void EntityMovement()
    {
        if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
        {
            Debug.Log("Can set new path!");
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

    private IEnumerator RegenStaminaRoutine()
    {
        _isStaminaRegening = true;
        _isStaying = true;
        _agent.ResetPath();
        
        while (_currentEntityStamina < MaxEntityStamina)
        {
            _currentEntityStamina += (regeningStaminaAmount * Time.deltaTime);

            yield return new WaitForSeconds(0.4f);
        }

        _currentEntityStamina = Mathf.Clamp(_currentEntityStamina, 0f, MaxEntityStamina);
        _isStaminaRegening = false;
        _isStaying = false;
        SwitchEntityStates();
    }
}

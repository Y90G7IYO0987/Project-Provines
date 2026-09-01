using System.Collections;
using UnityEngine;

public class EntityVisual : MonoBehaviour
{
    [SerializeField] private EntityData entityData;
    [SerializeField] private float changingStaminaCount = 150.0f;
    [SerializeField] private float regeningStaminaAmount = 140.0f;

    private EntityCharacteristics _entityCharacteristics;
    private TransferEntityCharacteristics _transferCharacteristics;
    private EntityController _entityController;

    private float maxEntityStamina;

    public float GetCurrentStamina() => _entityCharacteristics.GetCurrentStamina();

    private void Awake()
    {
        maxEntityStamina = entityData.MaxStamina;

        _transferCharacteristics = new TransferEntityCharacteristics()
        {
            MaxEntityStamina = maxEntityStamina
        };

        _entityCharacteristics = new EntityCharacteristics(_transferCharacteristics);

        _entityController = GetComponent<EntityController>();
    }

    public void ChangeStamina()
    {
        if (_entityCharacteristics.EntityRegeningStamina) return;

        if (_entityCharacteristics.EntityStamina == 0f)
        {
            StartCoroutine(RegenStaminaRoutine());

            return;
        }

        float changeAmount = -changingStaminaCount * Time.deltaTime;

        _entityCharacteristics.ChangeStamina(changeAmount);
    }

    private IEnumerator RegenStaminaRoutine()
    {
        Debug.Log($"Start regening stamina.");

        _entityCharacteristics.ChangeStaminaRegen(true);

        _entityController.SetStayingState(true);
        _entityController.ResetAgentPath();

        float currentEntityStamina = _entityCharacteristics.GetCurrentStamina();

        Debug.Log($"Current entity stamina - {currentEntityStamina}.");

        while (currentEntityStamina < maxEntityStamina)
        {
            float regenAmount = regeningStaminaAmount * Time.deltaTime;
            _entityCharacteristics.ChangeStamina(regenAmount);
            currentEntityStamina = _entityCharacteristics.GetCurrentStamina();

            yield return new WaitForSeconds(0.4f);
        }

        Debug.Log($"Final stamina - {currentEntityStamina}");

        _entityCharacteristics.ChangeStaminaRegen(false);

        _entityController.SetStayingState(false);
        _entityController.SwitchEntityStates();
    }
}

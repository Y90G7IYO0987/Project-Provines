using UnityEngine;

public struct TransferEntityCharacteristics
{
    public float MaxEntityStamina;
}

public class EntityCharacteristics
{
    public bool EntityRegeningStamina { get; private set; }
    public float MaxEntityStamina { get; private set; }
    public float EntityStamina { get; private set; }

    public void ChangeStaminaRegen(bool isRegening) => EntityRegeningStamina = isRegening;
    public float GetCurrentStamina() => EntityStamina;
    
    public EntityCharacteristics(TransferEntityCharacteristics characteristicsData)
    {
        MaxEntityStamina = characteristicsData.MaxEntityStamina;
        EntityStamina = MaxEntityStamina;
    }

    public void ChangeStamina(float amount)
    {
        EntityStamina += amount;
        EntityStamina = Mathf.Clamp(EntityStamina, 0f, MaxEntityStamina);
    }
}

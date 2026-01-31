using UnityEngine;

[System.Serializable]
public class CardMessage
{
    public MessageType type;
    public int value; // Used for Damage/Heal amount, or other numeric effects
    // Optional: Add more parameters as needed, e.g., for buffs/debuffs in future expansions
    // public float duration; // For timed effects
    // public string sourceCardName; // For logging or effects based on source

    public CardMessage(MessageType messageType, int messageValue = 0)
    {
        type = messageType;
        value = messageValue;
    }

    /// <summary>
    /// Applies the message effect to the given CardStatus.
    /// </summary>
    public void Apply(CardStatus status)
    {
        if (status == null)
        {
            Debug.LogWarning("Cannot apply CardMessage: CardStatus is null.");
            return;
        }

        switch (type)
        {
            case MessageType.Damage:
                status.TakeDamage(value);
                break;
            case MessageType.Heal:
                status.Heal(value);
                break;
            case MessageType.Destroy:
                // Instantly defeat the card by dealing max HP damage
                status.TakeDamage(status.GetMaxHP());
                break;
            // Add more cases for additional types as needed
            default:
                Debug.LogWarning($"Unhandled MessageType: {type}");
                break;
        }
    }
}

public enum MessageType
{
    Damage,
    Destroy,
    Heal,
    // Expand with more types as needed, e.g., BuffAttack, DebuffAttack, LevelUp
}
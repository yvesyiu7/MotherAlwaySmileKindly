using System;
using UnityEngine;

[Serializable]
public class CardStatus
{
    // Core parameters for the card status
    [SerializeField] protected int currentHP;
    [SerializeField] protected int maxHP;
    [SerializeField] protected int level;
    [SerializeField] protected int attack;

    // Optional: Events or delegates for status changes (e.g., for UI updates)
    public System.Action<int> OnHPChanged;
    public System.Action<int> OnLevelChanged;
    public System.Action<int> OnAttackChanged;

    public virtual void Init(CardCsvData cardCsvData)
    {
        currentHP = cardCsvData.Hp;
        maxHP = cardCsvData.Hp;
        level = 1;
        attack = cardCsvData.Attack;
    }

    // Method to initialize status (call from CardRuntime or subclass)

    public virtual void InitializeStatus(int initialMaxHP, int initialLevel, int initialAttack)
    {
        maxHP = initialMaxHP;
        currentHP = maxHP;
        level = initialLevel;
        attack = initialAttack;

        OnHPChanged?.Invoke(currentHP);
        OnLevelChanged?.Invoke(level);
        OnAttackChanged?.Invoke(attack);
    }

    // Method to take damage
    public virtual void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        OnHPChanged?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            // Handle death or defeat (override in subclasses if needed)
            Debug.Log("Card defeated!");
            // e.g., Destroy(gameObject);
        }
    }

    // Method to heal
    public virtual void Heal(int healAmount)
    {
        currentHP += healAmount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        OnHPChanged?.Invoke(currentHP);
    }

    // Method to level up
    public virtual void LevelUp(int levelsToAdd = 1)
    {
        level += levelsToAdd;
        // Optionally scale stats with level
        maxHP += 10 * levelsToAdd; // Example scaling
        attack += 5 * levelsToAdd; // Example scaling
        currentHP = maxHP; // Full heal on level up

        OnLevelChanged?.Invoke(level);
        OnAttackChanged?.Invoke(attack);
        OnHPChanged?.Invoke(currentHP);
    }

    // Getters for status values
    public int GetCurrentHP() => currentHP;
    public int GetMaxHP() => maxHP;
    public int GetLevel() => level;
    public int GetAttack() => attack;

    // Setter for attack (if needed dynamically)
    public void SetAttack(int newAttack)
    {
        attack = newAttack;
        OnAttackChanged?.Invoke(attack);
    }
}
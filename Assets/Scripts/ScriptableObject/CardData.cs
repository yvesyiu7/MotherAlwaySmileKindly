// CardData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Basic Info")]
    public string title = "Card Name";

    [TextArea(3, 6)]
    public string description = "Does something cool...";

    [Header("Visuals")]
    public Sprite artwork;              // ¡ö main card image

    [Header("Stats")]
    public int hp = 5;
    public int attack = 3;

    // Optional extras you might want later
    [Header("Gameplay (optional)")]
    public int manaCost = 4;
    public CardType type = CardType.Creature;   // enum below
    public Rarity rarity = Rarity.Common;

    // You can add more later: flavor text, abilities list, etc.
}

public enum CardType
{
    Creature,
    Spell,
    Item,
    Trap,
    Hero
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
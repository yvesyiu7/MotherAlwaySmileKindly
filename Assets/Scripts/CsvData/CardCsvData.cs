using System;

// For CSV parsing clarity (you can also use just plain class)
[Serializable]
public class CardCsvData
{
    public int CardId { get; set; }           // 1001, 1002, ...
    public string CardType { get; set; }      // "Monster", "Spell", "Trap", "Item", etc.
    public string Rarity { get; set; }        // "Common", "Rare", "Epic", "Legendary", "Mythic"
    public string ImageId { get; set; }       // "card_1001", "fire_dragon", or just number "017"
    public int CardTitleId { get; set; }      // localization key (often int for I2Loc / Unity Localization)
    public int DescriptionId { get; set; }    // localization key for description

    // Optional: constructor for easier debugging / creation in code
    public CardCsvData()
    {
        CardId = 0;
        CardType = "";
        Rarity = "Common";
        ImageId = "";
        CardTitleId = 0;
        DescriptionId = 0;
    }

    // Helpful for debugging
    public override string ToString()
    {
        return $"Card {CardId} | {Rarity} {CardType} | TitleID:{CardTitleId} | Img:{ImageId}";
    }
}
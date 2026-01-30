using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CardCSVManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string csvFileName = "CardDatabase.csv";   // placed in Resources folder

    // Main storage - access this from anywhere
    public static Dictionary<int, CardCsvData> Cards { get; private set; }
        = new Dictionary<int, CardCsvData>();

    // Singleton pattern (optional but very common for managers)
    public static CardCSVManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCardData();
    }

    private void LoadCardData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(csvFileName));

        if (csvFile == null)
        {
            Debug.LogError($"CSV file not found in Resources: {csvFileName}");
            return;
        }

        Cards.Clear();

        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Skip header row (assuming first line is header)
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = ParseCSVLine(line);

            if (values.Length < 6)
            {
                Debug.LogWarning($"Invalid line {i + 1}: {line}");
                continue;
            }

            try
            {
                var card = new CardCsvData
                {
                    CardId = int.Parse(values[0]),
                    CardType = values[1].Trim(),
                    Rarity = values[2].Trim(),
                    ImageId = values[3].Trim(),
                    CardTitleId = int.Parse(values[4]),
                    DescriptionId = int.Parse(values[5])
                };

                if (Cards.ContainsKey(card.CardId))
                {
                    Debug.LogWarning($"Duplicate CardId found: {card.CardId}");
                }
                else
                {
                    Cards.Add(card.CardId, card);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error parsing line {i + 1}: {line}\n{e.Message}");
            }
        }

        Debug.Log($"Loaded {Cards.Count} cards from {csvFileName}");
    }

    // Handles commas inside quoted fields (very simple version)
    private string[] ParseCSVLine(string line)
    {
        // Very basic CSV split - doesn't handle escaped quotes
        // For production you might want to use CsvHelper or better parser
        return line.Split(',')
                   .Select(s => s.Trim().Trim('"'))
                   .ToArray();
    }

    // ¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w
    // Helper methods you will probably use a lot
    // ¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w

    public static bool TryGetCard(int cardId, out CardCsvData card)
    {
        return Cards.TryGetValue(cardId, out card);
    }

    public static CardCsvData GetCard(int cardId)
    {
        Cards.TryGetValue(cardId, out var card);
        return card;
    }

    public static bool Exists(int cardId) => Cards.ContainsKey(cardId);
}
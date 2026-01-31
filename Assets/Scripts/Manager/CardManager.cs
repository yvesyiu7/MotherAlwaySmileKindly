using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    private Dictionary<int, GameObject> prefabCache = new Dictionary<int, GameObject>();
    private List<CardRuntime> activeCardsInScene = new List<CardRuntime>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // 1. Load resources from "Resources/Prefabs/Cards/"
        // Resources.LoadAll finds all prefabs at the path
        GameObject[] cardPrefabs = Resources.LoadAll<GameObject>("Prefabs/Cards/");

        foreach (GameObject prefab in cardPrefabs)
        {
            CardRuntime runtimeComp = prefab.GetComponent<CardRuntime>();
            if (runtimeComp != null)
            {
                // Cache it with CardId as the key
                if (!prefabCache.ContainsKey(runtimeComp.CardId))
                {
                    prefabCache.Add(runtimeComp.CardId, prefab);
                }
            }
        }

        // 2. Search for any CardRuntime already in the scene
        activeCardsInScene.AddRange(Object.FindObjectsByType<CardRuntime>(FindObjectsSortMode.None));
    }

    /// <summary>
    /// Instantiates a card prefab by ID and adds it to the active list.
    /// </summary>
    public CardRuntime CreateCard(int cardId, Vector3 position, Quaternion rotation)
    {
        if (prefabCache.TryGetValue(cardId, out GameObject prefab))
        {
            GameObject newObj = Instantiate(prefab, position, rotation);
            CardRuntime runtime = newObj.GetComponent<CardRuntime>();

            if (runtime != null)
            {
                activeCardsInScene.Add(runtime);
                return runtime;
            }
        }

        Debug.LogWarning($"Card ID {cardId} not found in prefab cache.");
        return null;
    }

    /// <summary>
    /// Returns the list of all active CardRuntime objects in the scene.
    /// </summary>
    public List<CardRuntime> GetAllCards()
    {
        return activeCardsInScene;
    }

    /// <summary>
    /// Destroys the card gameobject and removes it from the tracking list.
    /// </summary>
    public void DestroyCard(CardRuntime card)
    {
        if (card == null) return;

        if (card != null && activeCardsInScene.Contains(card))
        {
            activeCardsInScene.Remove(card);
        }

        Destroy(card.gameObject);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance { get; private set; }

    // Dictionaries to cache your sprites
    private Dictionary<string, Sprite> cardImages = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> cardBorders = new Dictionary<string, Sprite>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // Load and cache all sprites from specified Resources paths
        LoadSpritesToCache("Sprite/CardImages", cardImages);
        LoadSpritesToCache("Sprite/CardBorder", cardBorders);
        
    }

    private void LoadSpritesToCache(string path, Dictionary<string, Sprite> cache)
    {
        // Resources.LoadAll finds all assets of type Sprite in the folder
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(path);

        foreach (Sprite s in loadedSprites)
        {
            if (!cache.ContainsKey(s.name))
            {
                cache.Add(s.name, s);
            }
        }

        Debug.Log($"Loaded {loadedSprites.Length} sprites from: {path}");
    }

    public Sprite GetCardImage(string spriteName) =>
        GetSpriteFromCache(spriteName, cardImages, "CardImages");

    public Sprite GetCardBorder(string spriteName) =>
        GetSpriteFromCache(spriteName, cardBorders, "CardBorder");

    private Sprite GetSpriteFromCache(string name, Dictionary<string, Sprite> cache, string category)
    {
        if (cache.TryGetValue(name, out Sprite sprite))
        {
            return sprite;
        }

        // Logs a warning to the Unity Console if the sprite isn't found
        Debug.LogWarning($"[SpriteManager] {category} Sprite '{name}' not found in cache! Check folder path or spelling.");
        return null;
    }
}

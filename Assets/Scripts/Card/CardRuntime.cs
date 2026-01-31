using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MovementController))]
public class CardRuntime : MonoBehaviour
{
    // TextMeshPro components for card text (assuming world-space TextMeshPro)
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI hpText;

    // SpriteRenderer components for card visuals
    public SpriteRenderer cardImage;
    public SpriteRenderer cardBorder;
    public SpriteRenderer cardTypeIcon;

    // Physics components (required)
    protected Rigidbody rb;
    protected BoxCollider boxCollider;

    // Transform for the visual model (e.g., a child object holding visuals)
    public Transform visualModel;

    // Reference to the MovementController script
    protected MovementController movementController;

    protected virtual void Awake()
    {
        // Get required components if not assigned
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        movementController = GetComponent<MovementController>();

        // Optional: Validate other components (you can assign them in the Inspector or via code in subclasses)
        if (titleText == null) Debug.LogWarning("Title TextMeshPro not assigned.");
        if (hpText == null) Debug.LogWarning("HP TextMeshPro not assigned.");
        if (cardImage == null) Debug.LogWarning("Card Image SpriteRenderer not assigned.");
        if (cardBorder == null) Debug.LogWarning("Card Border SpriteRenderer not assigned.");
        if (cardTypeIcon == null) Debug.LogWarning("Card Type Icon SpriteRenderer not assigned.");
        if (visualModel == null) Debug.LogWarning("Visual Model Transform not assigned.");
    }

    // Base method to set card data (override in subclasses)
    public virtual void InitializeCard(string title, string description, int hp, Sprite imageSprite, Sprite borderSprite, Sprite typeIconSprite)
    {
        if (titleText != null) titleText.text = title;
        if (hpText != null) hpText.text = hp.ToString();
        if (cardImage != null) cardImage.sprite = imageSprite;
        if (cardBorder != null) cardBorder.sprite = borderSprite;
        if (cardTypeIcon != null) cardTypeIcon.sprite = typeIconSprite;
    }

    // Example base update (override if needed)
    protected virtual void Update()
    {
        // Base behavior, e.g., handle movement via controller
        if (movementController != null)
        {
            // Add logic here if needed, or handle in MovementController
        }
    }

}
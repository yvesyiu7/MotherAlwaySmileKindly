using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MovementController))]
public class CardRuntime : MonoBehaviour
{
    public int CardId;
    private CardCsvData _cardCsvData;
    public CardStatus CardStatus;

    // TextMeshPro components for card text (assuming world-space TextMeshPro)
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI effectText;

    // SpriteRenderer components for card visuals
    public SpriteRenderer cardImage;
    public SpriteRenderer cardBorder;
    public SpriteRenderer cardTypeIcon;

    // Physics components (required)
    protected Rigidbody rb;
    protected BoxCollider boxCollider;
    [SerializeField] protected AudioSource hitSound;

    // Transform for the visual model (e.g., a child object holding visuals)
    public Transform visualModel;

    // Reference to the MovementController script

    [SerializeField] protected AnimationController visualModelAnimator;
    [SerializeField] protected MovementController movementController;
    protected AnimationController animationController;
    protected DraggableObject draggableObject;

    protected virtual void Awake()
    {
        // Get required components if not assigned
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        visualModelAnimator = visualModel.GetComponent<AnimationController>();
        movementController = GetComponent<MovementController>();
        animationController = GetComponent<AnimationController>();

        // Optional: Validate other components (you can assign them in the Inspector or via code in subclasses)
        if (titleText == null) Debug.LogWarning("Title TextMeshPro not assigned.");
        if (hpText == null) Debug.LogWarning("HP TextMeshPro not assigned.");
        if (cardImage == null) Debug.LogWarning("Card Image SpriteRenderer not assigned.");
        if (cardBorder == null) Debug.LogWarning("Card Border SpriteRenderer not assigned.");
        if (cardTypeIcon == null) Debug.LogWarning("Card Type Icon SpriteRenderer not assigned.");
        if (visualModel == null) Debug.LogWarning("Visual Model Transform not assigned.");
        cardTypeIcon.gameObject.SetActive(false);
    }

    protected virtual void Start()
    {
        if (CardId != 0) {
            InitializeCard(CardCSVManager.GetCard(CardId));
        }

    }

    // Base method to set card data (override in subclasses)
    public virtual void InitializeCard(CardCsvData cardCsvData)
    {
        _cardCsvData = cardCsvData;
        CardStatus.Init(this, cardCsvData);
        RefreshUI();
    }

    protected void OnHPChanged(int i)
    {
        RefreshUI();
    }

    public virtual void RefreshUI()
    {
        titleText.text = _cardCsvData.CardTitleId;
        hpText.text = CardStatus.GetCurrentHP().ToString();
        cardImage.sprite = SpriteManager.Instance.GetCardImage(_cardCsvData.ImageId);
        cardBorder.sprite = SpriteManager.Instance.GetCardBorder(_cardCsvData.Rarity);
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

    protected virtual void SendCardMessage(CardRuntime target)
    {

    }

    public virtual void ReceiveMessage(CardMessage cardMessage)
    {
        switch (cardMessage.type)
        {
            case MessageType.Damage:
                CardStatus.ReceiveMessage(cardMessage);
                DoRed();
                PlayEffectText(cardMessage);
                if(hitSound != null)
                {
                    hitSound.Play();
                }
                break;
            case MessageType.Heal:
                CardStatus.ReceiveMessage(cardMessage);
                break;
            case MessageType.Destroy:
                DestroyCard();
                PlayEffectText(cardMessage);

                break;
            default:
                break;
        }
        RefreshUI();
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Heal"))
        {
            CardRuntime card = collision.gameObject.GetComponent<CardRuntime>();
            CardMessage cardMessage = new CardMessage(MessageType.Heal, 5);
            PlayEffectText(cardMessage);
            ReceiveMessage(cardMessage);
            CardManager.Instance.DestroyCard(card);
        }

        if (collision.gameObject.CompareTag("Metals"))
        {
            
            CardManager.Instance.DestroyCard(this);
            
        }
        if (collision.gameObject.CompareTag("KitchenUtensils"))
        {
            CardManager.Instance.DestroyCard(this);
            var card = CardManager.Instance.CreateCard(1011, transform.position);
            card.ScaleUpFrom0();
        }
        if (collision.gameObject.CompareTag("Hkstool"))
        {
            CardManager.Instance.DestroyCard(this);
            var card = CardManager.Instance.CreateCard(1010, transform.position);
            card.ScaleUpFrom0();
        }
    }

    public virtual void On0Hp()
    {
        DestroyCard();
    }

    public virtual void DestroyCard()
    {
        CardManager.Instance.DestroyCard(this);

    }

    public void DoRed()
    {
        cardImage.DOKill();
        cardImage.color = Color.white;
        cardImage.DOColor(Color.red, 0.2f)
                   .SetLoops(2, LoopType.Yoyo);
    }

    public void PlayEffectText(CardMessage message)
    {
        // 1. Reset: Snap back to original position and full visibility immediately

        effectText.DOKill();
        effectText.transform.localPosition = Vector3.zero;
        effectText.alpha = 1f;
        switch (message.type)
        {
            case MessageType.Damage:
                effectText.color = Color.red;
                effectText.text = "-" + message.value.ToString();
                break;
            case MessageType.Destroy:
                break;
            case MessageType.Heal:
                effectText.color = Color.green;
                effectText.text = "+" + message.value.ToString();
                break;
            default:
                break;
        }

        // 2. Animate: Create a sequence to move and fade simultaneously
        effectText.DOFade(0f, 1.5f);
            effectText.transform.DOLocalMoveY(50f, 0.5f).SetEase(Ease.OutExpo);
        /*Sequence textSeq = DOTween.Sequence();
        textSeq.Join(effectText.DOFade(0f, 1.5f));
        // Move up by 50 units on the Y axis
        textSeq.Join();*/

        // Fade out to 0 alpha
    }

    public void ScaleUpFrom0()
    {
        animationController.ScaleUp();
    }


}
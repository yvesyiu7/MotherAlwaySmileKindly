using System.Collections.Generic;
using UnityEngine;

public class EnemyRuntime : CardRuntime
{
    [Header("Settings")]
    public float attackRange = 2f;

    [Header("References")]
    public MovementController movementController; // Assign in Inspector

    protected override void Update()
    {
        base.Update();
        LookForTarget();
    }

    public void LookForTarget()
    {
        // 1. Get the list of cards from CardManager
        List<CardRuntime> allCards = CardManager.Instance.GetAllCards();

        CardRuntime closestTarget = null;
        float minDistance = Mathf.Infinity;

        // 2. Iterate to find the closest card without the "Enemy" tag
        foreach (CardRuntime card in allCards)
        {
            if (card == null || card.CompareTag("Enemy")) continue;

            float distance = Vector3.Distance(transform.position, card.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = card;
            }
        }

        // 3. Act on the found target
        if (closestTarget != null)
        {
            if (minDistance <= attackRange)
            {
                SendCardMessage(closestTarget);
            }
            else
            {
                // Move towards target position if out of range
                movementController.MoveTo(closestTarget.transform.position);
            }
        }
    }

    protected override void SendCardMessage(CardRuntime target)
    {
        Debug.Log("Attacking: " + target.name);
        target.ReceiveMessage(new CardMessage(MessageType.Damage, CardStatus.GetAttack()));

        // Add your damage/animation logic here
    }
}



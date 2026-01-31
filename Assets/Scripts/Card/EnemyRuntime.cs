using System.Collections.Generic;
using UnityEngine;

public class EnemyRuntime : CardRuntime
{
    [Header("Settings")]
    public float attackRange = 3f;

    [Header("Settings")]
    public float attackInterval = 2f;

    protected float nextAttackTime = 0;

    protected override void Update()
    {
        base.Update();
        LookForTarget();
    }

    public void LookForTarget()
    {
        if (nextAttackTime > Time.time) {
            return;
        }
        // 1. Get the list of cards from CardManager
        List<CardRuntime> allCards = CardManager.Instance.GetAllCards();

        CardRuntime closestTarget = null;
        float minDistance = Mathf.Infinity;

        // 2. Iterate to find the closest card without the "Enemy" tag
        foreach (CardRuntime card in allCards)
        {
            if (!card.CompareTag("Character")) continue;
            //Debug.Log($"enemy LookForTarget");

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
                nextAttackTime = Time.time + attackInterval;
                Attack(closestTarget);
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

    public virtual void Attack(CardRuntime closestTarget)
    {
        movementController.Reset();
        animationController.MoveToAndReturn(closestTarget.transform.position);
        SendCardMessage(closestTarget);
    }
}



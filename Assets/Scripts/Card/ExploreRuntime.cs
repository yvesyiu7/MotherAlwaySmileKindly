using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ExploreRuntime : CardRuntime
{
    public List<int> CreateCardIds;
    public float RandomNewCardOffset;
    public override void On0Hp()
    {
        foreach (var cardId in CreateCardIds)
        {
            Vector3 pos = GetRandomPositionAroundCircle(RandomNewCardOffset);
            var card = CardManager.Instance.CreateCard(cardId, pos);
            card.ScaleUpFrom0();
        }
        base.On0Hp();
    }

    public Vector3 GetRandomPositionAroundCircle(float radius)
    {
        // 1. Get a random direction on a 2D unit circle
        // Using .normalized ensures the point is on the edge, not inside
        Vector2 randomPoint = Random.insideUnitCircle;

        // 2. Scale by radius and convert to 3D (X and Z for ground-plane, or X and Y for 2D)
        Vector3 offset = new Vector3(randomPoint.x, 0, randomPoint.y) * radius;

        // 3. Add to current transform.position to center it around this object
        return transform.position + offset;
    }
}

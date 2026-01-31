using UnityEngine;

public class ExploreDraggable : DraggableObject
{
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        cardRuntime.ReceiveMessage(new CardMessage(MessageType.Damage, 1));
    }

    public virtual void On0Hp()
    {

    }
}

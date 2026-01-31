using UnityEngine;

public class FoodRuntime : CardRuntime
{
    protected override void SendCardMessage(CardRuntime target)
    {
        Debug.Log("SendCardMessage: " + target.name);
        target.ReceiveMessage(new CardMessage(MessageType.Heal, CardStatus.GetAttack()));

        // Add your damage/animation logic here
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Character")) 
        {
            SendCardMessage(collision.gameObject.GetComponent<CardRuntime>());
            CardManager.Instance.DestroyCard(this);
        }
    }
}

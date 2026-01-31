using UnityEngine;

public class MedicalRuntime : CardRuntime
{
    protected override void SendCardMessage(CardRuntime target)
    {
        Debug.Log("SendCardMessage: " + target.name);

        target.ReceiveMessage(new CardMessage(MessageType.Heal, CardStatus.GetAttack()));

        // Add your damage/animation logic here
    }
}

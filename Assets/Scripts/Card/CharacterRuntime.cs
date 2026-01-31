using UnityEngine;

public class CharacterRuntime : CardRuntime
{

    protected override void SendCardMessage(CardRuntime target)
    {
        Debug.Log("Attacking: " + target.name);
        target.ReceiveMessage(new CardMessage(MessageType.Damage, CardStatus.GetAttack()));

        // Add your damage/animation logic here
    }
}

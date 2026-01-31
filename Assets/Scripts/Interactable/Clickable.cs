using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Placeholder function: Add your custom logic here when the object is clicked
        Debug.Log("Object clicked: ");
        // For example, you could trigger an animation, sound, or other event
    }
}
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private bool canDrag;
    private bool isFirstClick = true;
    private Vector3 screenPoint;
    private Vector3 offset;

    private AnimationController controller;
    private void Start()
    {
        controller = GetComponent<AnimationController>();
    }

    private void OnMouseDown()
    {
        // Handle the clickable part: Log only on the first click
        if (isFirstClick)
        {
            Debug.Log("First click detected on " + gameObject.name);
            isFirstClick = false;
        }

        // Prepare for dragging: Calculate offset to maintain relative position
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        offset = transform.position - worldPos;

        if (controller != null) {
            controller.Squeeze();
        }
    }

    private void OnMouseDrag()
    {
        if (!canDrag) {
            return;
        }
        // Handle dragging: Move the object to the mouse position in x and z, keeping y the same
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 newPos = worldPos + offset;
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
    }
}
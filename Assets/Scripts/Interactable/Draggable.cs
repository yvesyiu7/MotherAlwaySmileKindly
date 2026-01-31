using UnityEngine;
using UnityEngine.Events; // Required for UnityEvents

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private bool canDrag = true;
    private bool isFirstClick = true;
    private Vector3 screenPoint;
    private Vector3 offset;

    // Hook these up in the Inspector!
    public UnityEvent OnDragStarted;
    public UnityEvent OnDragEnded;

    private AnimationController controller;
    protected CardRuntime cardRuntime;

    private void Awake()
    {
        controller = GetComponent<AnimationController>();
        cardRuntime = GetComponent<CardRuntime>();
    }

    protected virtual void OnMouseDown()
    {
        if (isFirstClick)
        {
            Debug.Log("First click detected on " + gameObject.name);
            isFirstClick = false;
        }

        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        offset = transform.position - worldPos;

        if (controller != null) controller.Squeeze();

        // Trigger the hook!
        OnDragStarted?.Invoke();
    }

    protected virtual void OnMouseDrag()
    {
        if (!canDrag) return;

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 newPos = worldPos + offset;
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
    }

    protected virtual void OnMouseUp()
    {

        // Trigger the hook!
        OnDragEnded?.Invoke();
    }
}

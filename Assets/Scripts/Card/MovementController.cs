using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Basic movement settings (expand as needed)
    public float moveSpeed = 5f;
    public Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;

    // New variables for target-based movement
    private Vector3 targetPosition;
    private bool isMovingToTarget = false;
    public float arrivalThreshold = 0.1f; // Distance threshold to consider arrived at target

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isMovingToTarget)
        {
            // Calculate direction to target
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            moveDirection = directionToTarget;

            // Move towards the target
            if (rb != null && moveDirection != Vector3.zero)
            {
                rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            }

            // Check if we've arrived at the target
            if (Vector3.Distance(transform.position, targetPosition) <= arrivalThreshold)
            {
                isMovingToTarget = false;
                moveDirection = Vector3.zero;
                // Optional: You could add an event or callback here if needed, e.g., OnArrival()
            }
        }
        else
        {
            // Existing basic movement logic (if not moving to target)
            if (rb != null && moveDirection != Vector3.zero)
            {
                rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    // Existing method to set movement direction (call from CardRuntime or subclasses)
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
        // If setting manual direction, disable target movement to avoid conflicts
        isMovingToTarget = false;
    }

    // New method to move to a specific destination
    public void MoveTo(Vector3 destination)
    {
        targetPosition = destination;
        isMovingToTarget = true;
    }

    public void Reset()
    {
        isMovingToTarget = false;
        moveDirection = Vector3.zero;
    }
}
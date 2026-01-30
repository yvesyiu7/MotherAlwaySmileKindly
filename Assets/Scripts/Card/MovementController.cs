using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Basic movement settings (expand as needed)
    public float moveSpeed = 5f;
    public Vector3 moveDirection = Vector3.zero;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Example basic movement (you can control via input or AI in subclasses)
        if (rb != null && moveDirection != Vector3.zero)
        {
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Method to set movement direction (call from CardRuntime or subclasses)
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }
}
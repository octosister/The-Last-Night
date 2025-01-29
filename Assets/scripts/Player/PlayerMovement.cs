using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float groundCheckDistance = 0.1f; // Distance to check for ground
    public LayerMask groundLayer; // Layer mask for ground objects

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        // Get the Rigidbody and CapsuleCollider components
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (rb == null || capsuleCollider == null)
        {
            Debug.LogError("Rigidbody or CapsuleCollider component not found on the player!");
        }

        // Freeze rotation to prevent unwanted tilting
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Get input from the player
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveVertical = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow

        // Calculate movement direction relative to the player's orientation
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;

        // Normalize the movement vector to prevent faster diagonal movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Apply movement to the Rigidbody
        MovePlayer(movement);
    }

    void MovePlayer(Vector3 movement)
    {
        // Check if the player is grounded
        bool isGrounded = IsGrounded();

        if (isGrounded)
        {
            // Calculate the target velocity
            Vector3 targetVelocity = movement * speed;

            // Apply the movement force to the Rigidbody
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        }
    }

    bool IsGrounded()
    {
        // Perform a raycast to check if the player is on the ground
        RaycastHit hit;
        Vector3 rayStart = transform.position + capsuleCollider.center;
        float rayLength = capsuleCollider.height / 2 + groundCheckDistance;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, rayLength, groundLayer))
        {
            return true;
        }

        return false;
    }
}
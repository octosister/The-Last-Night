using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Transform playerCam;        // Camera or reference point for where the object will float
    public Transform holdPosition;     // The position in front of the player where the object will float
    public float pickUpDistance = 1f;  // Max distance from which you can pick up objects (1 meter)
    public float moveSpeed = 10f;      // Speed at which the object moves towards the hold position
    public float maxVelocity = 20f;    // Maximum velocity of the object when it's being held
    private GameObject heldObject;     // The object the player is holding
    private Rigidbody heldObjectRb;    // Rigidbody of the held object
    private Quaternion relativeRotation;  // To store the rotation relative to the player's camera

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left-click to pick up an object
        {
            TryPickUpObject();
        }

        if (Input.GetMouseButtonUp(0))  // Release the object when left-click is released
        {
            if (heldObject != null)
            {
                DropObject();
            }
        }
    }

    void FixedUpdate()
    {
        if (heldObject != null)  // Move object to the hold position while it's picked up
        {
            MoveObjectToHoldPosition();
        }
    }

    void TryPickUpObject()
    {
        // Raycast from the camera to detect objects in front of the player
        RaycastHit hit;
        if (Physics.Raycast(playerCam.position, playerCam.forward, out hit, pickUpDistance))
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("PickUp"))  // Ensure the object is tagged as "PickUp"
            {
                PickUpObject(hit.collider.gameObject);  // Pick up the object if within distance
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        heldObjectRb = heldObject.GetComponent<Rigidbody>();

        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = false;     // Disable gravity so it floats
            heldObjectRb.drag = 10;              // Increase drag to make movement smoother
            heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation;  // Prevent it from rotating

            // Set the Rigidbody interpolation to reduce lagging
            heldObjectRb.interpolation = RigidbodyInterpolation.Interpolate;

            // Calculate the relative rotation between the player camera and the object
            relativeRotation = Quaternion.Inverse(playerCam.rotation) * heldObject.transform.rotation;
        }
    }

    void MoveObjectToHoldPosition()
    {
        // Calculate the direction and velocity needed to move the object towards the hold position
        Vector3 directionToHoldPosition = (holdPosition.position - heldObject.transform.position).normalized;
        float distance = Vector3.Distance(heldObject.transform.position, holdPosition.position);

        // Calculate the velocity towards the hold position
        Vector3 targetVelocity = directionToHoldPosition * moveSpeed * distance;

        // Clamp the velocity to avoid it moving too fast
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, maxVelocity);

        // Apply the velocity to the object
        heldObjectRb.velocity = targetVelocity;

        // Update the object's rotation based on the player's camera rotation and the stored relative rotation
        heldObject.transform.rotation = playerCam.rotation * relativeRotation;
    }

    void DropObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = true;      // Re-enable gravity
            heldObjectRb.drag = 1;               // Reset drag to its original value
            heldObjectRb.constraints = RigidbodyConstraints.None;  // Remove any constraints

            // Keep the velocity of the object when released, allowing it to maintain momentum
            heldObjectRb.velocity = heldObjectRb.velocity;  // Momentum continues from current velocity

            // Reset interpolation to None for normal physics behavior after release
            heldObjectRb.interpolation = RigidbodyInterpolation.None;
        }

        heldObject = null;  // Clear reference to the held object
    }
}

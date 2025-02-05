using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float defaultWalkSpeed;
    private float sprintSpeedMultiplier = 1.5f;
    public float maxVelocity = 10f;
    public float forceMultiplier = 100f;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Vector3 moveDir;
    public Transform orientation;

    private float walkSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        walkSpeed = defaultWalkSpeed;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CapMaxVelocity();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            walkSpeed = defaultWalkSpeed * sprintSpeedMultiplier;
        }
        else
        {
            walkSpeed = defaultWalkSpeed;
        }

        moveDir = (orientation.right * Input.GetAxisRaw("Horizontal") + orientation.forward * Input.GetAxisRaw("Vertical")).normalized;

        rb.AddForce(moveDir * walkSpeed * forceMultiplier * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    void CapMaxVelocity()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > maxVelocity)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxVelocity;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }
}
using UnityEngine;
using KinematicCharacterController;

public class PlayerMovement : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor Motor;
    public Camera PlayerCamera;
    public float MoveSpeed = 5f;
    public float LookSensitivity = 2f;
    public float JumpForce = 5f;

    private Vector3 _moveInput;
    private Vector2 _lookInput;
    private float _cameraPitch;
    private float _targetYaw; // Track horizontal rotation

    private void Start()
    {
        Motor.CharacterController = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInput();
        HandleCamera();
    }

private void HandleInput()
{
    Vector3 rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

    if (rawInput.magnitude < 0.1f)
    {
        rawInput = Vector3.zero;
    }

    _moveInput = Vector3.ClampMagnitude(rawInput, 1f);

    _lookInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * LookSensitivity;

    if (Input.GetButtonDown("Jump") && Motor.GroundingStatus.IsStableOnGround)
    {
        Motor.ForceUnground();
        Motor.BaseVelocity = new Vector3(Motor.BaseVelocity.x, JumpForce, Motor.BaseVelocity.z);
    }
}

    private void HandleCamera()
    {
        // Horizontal rotation (stored for motor)
        _targetYaw += _lookInput.x;

        // Vertical rotation (applied to camera)
        _cameraPitch -= _lookInput.y;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);
        PlayerCamera.transform.localEulerAngles = Vector3.right * _cameraPitch;
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        Vector3 moveDirection = transform.TransformDirection(_moveInput) * MoveSpeed;
        currentVelocity = new Vector3(moveDirection.x, currentVelocity.y, moveDirection.z);

        if (!Motor.GroundingStatus.IsStableOnGround)
        {
            currentVelocity += Physics.gravity * deltaTime;
        }
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        // Apply horizontal rotation to the motor
        currentRotation = Quaternion.Euler(0f, _targetYaw, 0f);
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
        // Optional: Code to run before the motor updates
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        // Optional: Code to run after the motor updates grounding status
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
        // Optional: Code to run after the motor updates
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
        // Optional: Handle discrete collisions
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        // Optional: Handle movement hits
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
        // Optional: Process hit stability reports
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        // Optional: Handle ground hits
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        // Optional: Define custom logic for valid colliders
        return true;
    }
}
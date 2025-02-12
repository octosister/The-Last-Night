using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CapsuleVisualUpdater : MonoBehaviour
{
    [Tooltip("The Transform of the visual mesh that should match the capsule collider.")]
    public Transform visualMesh;

    // Default dimensions for Unity's built-in capsule:
    private const float DefaultCapsuleHeight = 2f;
    private const float DefaultCapsuleRadius = 0.5f;

    private CapsuleCollider capsuleCollider;

    // Store previous values to update only when needed
    private float previousHeight;
    private float previousRadius;

    private void Awake()
    {
        // Get the CapsuleCollider component
        capsuleCollider = GetComponent<CapsuleCollider>();

        // If no visual mesh is assigned, try to auto-assign the first child transform
        if (visualMesh == null && transform.childCount > 0)
        {
            visualMesh = transform.GetChild(0);
        }

        // Initialize previous values
        previousHeight = capsuleCollider.height;
        previousRadius = capsuleCollider.radius;
    }

    private void Start()
    {
        UpdateVisualMesh();
    }

    private void Update()
    {
        // Optionally, update the visual mesh if the collider dimensions have changed at runtime.
        if (!Mathf.Approximately(capsuleCollider.height, previousHeight) ||
            !Mathf.Approximately(capsuleCollider.radius, previousRadius))
        {
            UpdateVisualMesh();
            previousHeight = capsuleCollider.height;
            previousRadius = capsuleCollider.radius;
        }
    }

#if UNITY_EDITOR
    // This method updates the visual mesh in the Editor when you change values in the Inspector.
    private void OnValidate()
    {
        // In edit mode, try to get the collider if it hasn't been assigned yet.
        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider>();

        // Auto-assign the visual mesh if not set.
        if (visualMesh == null && transform.childCount > 0)
        {
            visualMesh = transform.GetChild(0);
        }
        UpdateVisualMesh();
    }
#endif

    /// <summary>
    /// Updates the visual mesh's scale to match the capsule collider's height and radius.
    /// </summary>
    public void UpdateVisualMesh()
    {
        if (capsuleCollider == null || visualMesh == null)
        {
            Debug.LogWarning("CapsuleCollider or Visual Mesh is not assigned on " + gameObject.name);
            return;
        }

        // Calculate the new scaling factors based on the defaults.
        float scaleY = capsuleCollider.height / DefaultCapsuleHeight;
        float scaleXZ = capsuleCollider.radius / DefaultCapsuleRadius;

        // Apply scaling to the visual mesh. (Assumes your mesh was originally modeled to the default dimensions.)
        visualMesh.localScale = new Vector3(scaleXZ, scaleY, scaleXZ);
    }
}

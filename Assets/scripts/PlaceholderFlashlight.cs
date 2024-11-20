using UnityEngine;

public class FlashlightPlaceholder : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlight; // The spotlight component
    public float maxBrightness = 5f; // Brightness when far away
    public float minBrightness = 1f; // Brightness when very close
    public float maxDistance = 20f; // Max distance to consider for brightness adjustment
    public Camera playerCamera;

    [Header("Input Key")]
    public KeyCode toggleKey = KeyCode.F;

    private bool isOn = true; // Flashlight state
    private float targetBrightness; // The target intensity value
    private float smoothSpeed = 5f; // Speed of brightness smoothing

    void Start()
    {
        if (flashlight == null)
        {
            flashlight = GetComponent<Light>();
        }
    }

    void Update()
    {
        HandleFlashlightToggle();
        AdjustBrightness();
        UpdateFlashlightPosition();
    }

    private void HandleFlashlightToggle()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isOn = !isOn;
            flashlight.enabled = isOn;
        }
    }

    private void AdjustBrightness()
    {
        if (!isOn) return;

        RaycastHit hit;
        float distance = maxDistance;

        // Check if something is directly in front of the flashlight
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxDistance))
        {
            distance = hit.distance; // Use the distance to the object
        }

        // Calculate the target intensity based on the distance
        targetBrightness = Mathf.Lerp(minBrightness, maxBrightness, distance / maxDistance);

        // Smoothly transition the flashlight intensity to the target value
        flashlight.intensity = Mathf.Lerp(flashlight.intensity, targetBrightness, smoothSpeed * Time.deltaTime);
    }

    private void UpdateFlashlightPosition()
    {
        flashlight.transform.position = playerCamera.transform.position;
        flashlight.transform.rotation = playerCamera.transform.rotation;
    }
}

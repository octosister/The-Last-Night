using UnityEngine;

public class PlaceholderFlashlight : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlight; // The spotlight component
    public float maxBrightness = 500f; // Brightness when far away
    public float minBrightness = 100f; // Brightness when very close
    public float maxDistance = 20f; // Max distance to consider for brightness adjustment
    public Camera playerCamera;

    [Header("Input Key")]
    public KeyCode toggleKey = KeyCode.F;

    private bool isOn = true; // Flashlight state

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
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            distance = hit.distance; // Use the distance to the object
        }

        // Calculate the intensity based on the distance
        float brightness = Mathf.Lerp(minBrightness, maxBrightness, distance / maxDistance);
        flashlight.intensity = brightness;
    }

    private void UpdateFlashlightPosition()
    {
        flashlight.transform.position = playerCamera.transform.position;
        flashlight.transform.rotation = playerCamera.transform.rotation;
    }
}

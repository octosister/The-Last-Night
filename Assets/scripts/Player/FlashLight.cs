using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    public Camera playerCamera;
    private bool isFlashlightOn = false;
    private bool hasFlashlight = false;

    void Update()
    {
        if (hasFlashlight && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (isFlashlightOn)
        {
            UpdateFlashlightPosition();
        }
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        flashlight.enabled = isFlashlightOn;
    }

    void UpdateFlashlightPosition()
    {
        flashlight.transform.position = playerCamera.transform.position;
        flashlight.transform.rotation = playerCamera.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FlashlightPickup"))
        {
            hasFlashlight = true;
            Destroy(other.gameObject);
        }
    }
}

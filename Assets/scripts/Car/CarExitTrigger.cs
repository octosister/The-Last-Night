using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class CarExitTrigger : MonoBehaviour
{
    public GameObject player; // Player object
    public GameObject carRoot; // Root object of the car (with Rigidbody)
    public CanvasGroup fadeCanvas; // Canvas for fade effect
    public TextMeshProUGUI exitPrompt; // TMP Text for "Press E to exit the car"

    private Rigidbody carRigidbody; // Reference to the car's Rigidbody
    private bool isCarInside = false; // Is the car inside the trigger
    private bool isCarStopped = false; // Is the car stopped
    private bool isFading = false; // Is the screen fading

    void Start()
    {
        // Get the Rigidbody from the car root
        carRigidbody = carRoot.GetComponent<Rigidbody>();

        if (carRigidbody == null)
        {
            Debug.LogError("No Rigidbody found on the car root object!");
        }

        // Ensure the prompt is hidden at the start
        if (exitPrompt != null)
        {
            exitPrompt.gameObject.SetActive(true); // Ensure it's active
            exitPrompt.text = ""; // Start with no text
        }
    }

    void Update()
    {
        // Check if the player can exit the car and listen for the E key
        if (isCarInside && isCarStopped && !isFading && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeTransition());
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Check if the car's Collider is inside the trigger
        if (IsCarPart(other.gameObject))
        {
            isCarInside = true;

            // Check if the car is stopped
            if (carRigidbody != null && carRigidbody.velocity.magnitude < 0.05f)
            {
                isCarStopped = true;

                // Show the prompt
                if (exitPrompt != null)
                {
                    exitPrompt.text = "Press E to exit the car";
                }
            }
            else
            {
                isCarStopped = false;

                // Hide the prompt if the car is moving
                if (exitPrompt != null)
                {
                    exitPrompt.text = "";
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the car's Collider leaves the trigger
        if (IsCarPart(other.gameObject))
        {
            isCarInside = false;
            isCarStopped = false;

            // Hide the prompt
            if (exitPrompt != null)
            {
                exitPrompt.text = "";
            }
        }
    }

    private IEnumerator FadeTransition()
    {
        isFading = true;

        // Hide the prompt before transitioning
        if (exitPrompt != null)
        {
            exitPrompt.text = "";
        }

        // Fade to black
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            fadeCanvas.alpha = t;
            yield return null;
        }
        fadeCanvas.alpha = 1;
        exitPrompt.gameObject.SetActive(false);

        // Transition player
        player.SetActive(true);
        player.transform.position = carRoot.transform.position + new Vector3(2, 0, 0); // Offset for player spawn
        carRoot.SetActive(false);

        // Fade back in
        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            fadeCanvas.alpha = t;
            yield return null;
        }
        fadeCanvas.alpha = 0;

        // Ensure prompt stays hidden after fade
        if (exitPrompt != null)
        {
            exitPrompt.text = ""; // Clear text
        }

        isFading = false;
    }

    // Helper method to check if the collider is part of the car
    private bool IsCarPart(GameObject obj)
    {
        // Check if the object belongs to the car hierarchy
        return obj.transform.IsChildOf(carRoot.transform);
    }
}

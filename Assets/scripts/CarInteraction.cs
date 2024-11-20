using UnityEngine;

public class CarInteraction : MonoBehaviour
{
    private bool isPlayerNearby = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void Update()
    {
        // When "E" is pressed and the player is nearby, switch to car control
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }
}

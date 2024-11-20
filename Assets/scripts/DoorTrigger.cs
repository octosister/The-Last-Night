using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorController doorController;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) 
        {
            doorController.ToggleDoor();
        }
    }
}

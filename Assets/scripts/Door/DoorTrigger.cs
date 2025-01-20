using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorController doorController;

    bool doorOpened = false;
    void OnTriggerEnter(Collider other)
    {
    

    
        if (other.CompareTag("Player"))
     {
            if(doorOpened!=true)
        {
            if (doorController.IsOpen()) 
            {
                doorController.CloseDoor();
                doorOpened = true;
                doorController.LockDoor();
            }
        }
    }


}
}

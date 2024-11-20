using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;  
    public float openSpeed = 2f;   
    private bool isOpen = false;   
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        
        if (isOpen)
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * openSpeed);
    }

    
    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}

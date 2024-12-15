using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public float interactionDistance = 3f;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Transform player;
    private AudioSource audioSource;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure your player object has the 'Player' tag.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isOpen)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * openSpeed);
        }

        CheckForInteraction();
    }

    void CheckForInteraction()
    {
        if (player != null && Vector3.Distance(player.position, transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleDoor();
            }
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        Debug.Log("Door toggled. New state: " + (isOpen ? "Open" : "Closed"));

        if (isOpen && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
        else if (!isOpen && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
    public void CloseDoor()
{
    if (isOpen)
    {
        isOpen = false;
        Debug.Log("Door closed.");

        if (closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
}


    public bool IsOpen()
    {
        return isOpen;
    }
}

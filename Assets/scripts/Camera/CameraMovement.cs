using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform orientation;
    public float mouseSensitivity = 100f;
    private Vector2 cameraRotation, lerpedRotation;
    public float lerpSpeed = 2f; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cameraRotation.x -= mouseY;
        cameraRotation.y += mouseX;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f);



        lerpedRotation = Vector2.Lerp(lerpedRotation, cameraRotation, lerpSpeed);

        transform.localRotation = Quaternion.Euler(lerpedRotation.x, lerpedRotation.y, 0f);
        orientation.localRotation = Quaternion.Euler(0f, lerpedRotation.y, 0f);
    }
}

using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    //treba este dokoncit bug pri pozerani objektov a holdingu
    public float zoomFOV = 30f;
    public float normalFOV = 60f;
    public float zoomSpeed = 10f;

    private Camera cameraComponent;

    void Start()
    {
        cameraComponent = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, zoomFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
        }
    }
}

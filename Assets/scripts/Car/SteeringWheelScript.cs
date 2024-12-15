using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;

public class SteeringWheelScript : MonoBehaviour
{
    
    public UnityEngine.Quaternion rotLeft;
    public UnityEngine.Quaternion rotRight;

    // Update is called once per frame
    private UnityEngine.Quaternion currRot;
    void Update()
    {
        currRot = UnityEngine.Quaternion.Lerp(rotLeft, rotRight, 0.5f + Input.GetAxis("Horizontal")/2);
        transform.localRotation = UnityEngine.Quaternion.Lerp(transform.localRotation, currRot, Time.deltaTime * 20);
    }
}
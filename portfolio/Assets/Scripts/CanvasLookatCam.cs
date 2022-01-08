using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookatCam : MonoBehaviour
{
    public bool Opened;

    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}

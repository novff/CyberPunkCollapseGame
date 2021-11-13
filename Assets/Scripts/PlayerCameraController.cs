using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public float MouseSensitivity = 400F;
    //theres no rigidity
    public Transform PlayerBody;
    float XRotation = 0f;

    void Start() 
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    void Update()
        {
            float CameraMovementX = Input.GetAxisRaw("Mouse X") * MouseSensitivity * Time.deltaTime;
            float CameraMovementY = Input.GetAxisRaw("Mouse Y") * MouseSensitivity * Time.deltaTime;

            XRotation -= CameraMovementY;
            XRotation = Mathf.Clamp(XRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
            PlayerBody.Rotate(Vector3.up * CameraMovementX);        
        }
}

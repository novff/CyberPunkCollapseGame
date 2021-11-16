using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    public Transform PlayerTransform;
    void Update()
        {
            transform.position = PlayerTransform.transform.position;
        }
}
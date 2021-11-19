using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    public Transform PlayerTransform;
    private bool localPause;
    void Update()
        {
            transform.position = PlayerTransform.transform.position;
        }
}
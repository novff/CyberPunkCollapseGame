using UnityEngine.UI;
using UnityEngine;

public class DebugInfoScript : MonoBehaviour
{
    public Transform player;
    public Rigidbody rb;
    private int avgFrameRate;
    public Text FPSText;
    public Text XcoordText;
    public Text YcoordText;
    public Text ZcoordText;
    public Text VelocityText;
    public void Update ()
        {
            FPSCounter();

            PlayerDebugCoordinates();
            PlayerVelocity();
        }

    public void FPSCounter()
        {
            float current = 0;
            current = (int)(1f / Time.unscaledDeltaTime);
            avgFrameRate = (int)current;
            FPSText.text = avgFrameRate.ToString() + " FPS";
        }
    public void PlayerDebugCoordinates()
        {
            float PDCx,PDCy,PDCz = 0;
            PDCx = (float)player.position.x;
            PDCy = (float)player.position.y;
            PDCz = (float)player.position.z;
            XcoordText.text = "X: " + PDCx.ToString();
            YcoordText.text = "Y: " + PDCy.ToString();
            ZcoordText.text = "Z: " + PDCz.ToString();
        }
    public void PlayerVelocity()
        {
            Vector3 vel = rb.velocity;
            float rbvel = vel.magnitude;
            VelocityText.text = "Vel: " + rbvel.ToString();
        }
}


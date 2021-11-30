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
    public Text PlayerInputsText;
    public void Update ()
        {
            FPSCounter();

            PlayerDebugCoordinates();
            PlayerVelocity();
            PlayerInputs();
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
    private void PlayerInputs()
        {
            string JText = "";
            if(Input.GetButton("Jump")) JText = "Jump ";
            string WText = "";
            if(Input.GetKey(KeyCode.W)) WText = "W ";
            string AText = "";
            if(Input.GetKey(KeyCode.A)) AText = "A ";
            string SText = "";
            if(Input.GetKey(KeyCode.S)) SText = "S ";
            string DText = "";
            if(Input.GetKey(KeyCode.D)) DText = "D ";
            string CrouchText = "";
            if(Input.GetKey(KeyCode.LeftControl)) CrouchText = "Crouch ";

            PlayerInputsText.text = WText + AText + SText + DText + JText + CrouchText;

        }
}


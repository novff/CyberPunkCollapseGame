using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour 
{
    public Transform PlayerCam;
    public Transform Orientation;
    private Rigidbody rb;
    //camera control
    private float xRotation;
    private float Sensitivity = 50f;
    private float SensitivityMultiplier = 1f;
    //Movement control
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask Ground;
    private Vector3 normalVector = Vector3.up;
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;
    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    public Transform HeadCollider;
    bool HeadTouchesSealing;
    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.5f;
    public float jumpForce = 550f;
    public int JumpCount = 1;
    //Input
    float x, y;
    bool jumping, sprinting, crouching;
    //init
    void Start() 
        {
            rb = GetComponent<Rigidbody>();
            playerScale =  transform.localScale;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    private void FixedUpdate() 
        {
            HeadTouchesSealing = Physics.CheckSphere(HeadCollider.position, 0.5F, Ground);

            Movement();
        }
    private void Update() 
        {
            bool localPause = UIBehaviour.IsPaused;
            bool localInInventory = UIBehaviour.InInventory;
            if(!localPause)
                {
                    if(!localInInventory)
                        {
                            PlayerInput();
                            Look();
                        }
                }
        }
    //input registration
    private void PlayerInput() 
        {
            if (Input.GetKeyUp(KeyCode.R))
                {
                    string currentSceneName = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene(currentSceneName);
                }
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
            jumping = Input.GetButton("Jump");
            crouching = Input.GetKey(KeyCode.LeftControl);
            //Crouching
            if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    StartCrouch();
                }
            //if (Input.GetKeyUp(KeyCode.LeftControl))
            if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    StopCrouch();
                }
        }

    private void StartCrouch() 
        {
            transform.localScale = Vector3.Lerp(crouchScale, playerScale, Time.deltaTime);
            //transform.localScale = crouchScale;
            if(!HeadTouchesSealing)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                }
        }
    private void StopCrouch() 
        {
            transform.localScale = Vector3.Lerp(playerScale, crouchScale, Time.deltaTime * 32);
            
            transform.localScale = playerScale;
            //transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }

    private void Movement() 
        {
            //Extra gravity
            rb.AddForce(Vector3.down * Time.deltaTime * 10);
        
            //Find actual velocity relative to where player is looking
            Vector2 mag = FindVelRelativeToLook();
            float xMag = mag.x, yMag = mag.y;

            //Counteract sliding and sloppy movement
            CounterMovement(x, y, mag);
        
            //If holding jump && ready to jump, then jump
            if (readyToJump && jumping && JumpCount != 0) Jump();
            if (grounded) JumpCount = 1;
            //Set max speed
            float maxSpeed = this.maxSpeed;
        
        if (crouching && grounded && readyToJump) 
            {
                maxSpeed = this.maxSpeed / 4;
                //rb.AddForce(Vector3.down * Time.deltaTime);
                //return;
            }
        
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;


        float multiplier = 1f, multiplierV = 1f;
        // Movement in air
        if (!grounded) 
            {
                multiplier = 0.5f;
                multiplierV = 0.5f;//0.5
            }
        
        // Movement while sliding
        if (grounded && crouching) multiplierV = 0.5f;

        //Apply forces to move player
        rb.AddForce(Orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(Orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier * multiplierV);
    }

    private void Jump() 
        {
            
            if (JumpCount != 0 && readyToJump) 
                {
                    readyToJump = false;

                    //Add jump forces
                    rb.AddForce(Vector2.up * jumpForce * 1.5f);
                    rb.AddForce(normalVector * jumpForce * 0.5f);
                    
                    //If jumping while falling, reset y velocity.
                    Vector3 vel = rb.velocity;
                    if (rb.velocity.y < 0.5f) rb.velocity = new Vector3(vel.x, 0, vel.z);
                    else if (rb.velocity.y > 0) rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
                    JumpCount -= 1;

                    Invoke(nameof(ResetJump), jumpCooldown);
                }
        }
    private void ResetJump() 
        {
            readyToJump = true;
        }
    
    private void Look() 
        {
            float mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.fixedDeltaTime * SensitivityMultiplier;
            float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.fixedDeltaTime * SensitivityMultiplier;

            //Find current look rotation
            Vector3 rot = PlayerCam.transform.localRotation.eulerAngles;
            float desiredX = rot.y + mouseX;
            
            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Perform the rotations
            PlayerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            Orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        }

    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping) return;

        //Slow down sliding
        //if (crouching) 
        //    {        
        //        rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
        //        return;
        //    }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * Orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * Orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) 
            {
                float fallspeed = rb.velocity.y;
                Vector3 n = rb.velocity.normalized * maxSpeed;
                rb.velocity = new Vector3(n.x, fallspeed, n.z);
            }
    }

    public Vector2 FindVelRelativeToLook() 
        {
            float lookAngle = Orientation.transform.eulerAngles.y;
            float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

            float u = Mathf.DeltaAngle(lookAngle, moveAngle);
            float v = 90 - u;

            float magnitue = rb.velocity.magnitude;
            float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
            float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
            
            return new Vector2(xMag, yMag);
        }

    private bool IsFloor(Vector3 v)
        {
            float angle = Vector3.Angle(Vector3.up, v);
            return angle < maxSlopeAngle;
        }

    //Handle ground detection
    private bool cancellingGrounded;
    private void OnCollisionStay(Collision other) 
        {
            //Make sure we are only checking for walkable layers
            int layer = other.gameObject.layer;
            if (Ground != (Ground | (1 << layer))) return;

            //Iterate through every collision in a physics update
            for (int i = 0; i < other.contactCount; i++) 
                {
                    Vector3 normal = other.contacts[i].normal;
                    //FLOOR
                    if (IsFloor(normal)) 
                        {
                            grounded = true;
                            cancellingGrounded = false;
                            normalVector = normal;
                            CancelInvoke(nameof(StopGrounded));
                        }
                }   
            //Invoke ground/wall cancel, since we can't check normals with CollisionExit
            float delay = 3f;
            if (!cancellingGrounded) 
                {
                    cancellingGrounded = true;
                    Invoke(nameof(StopGrounded), Time.deltaTime * delay);
                }
        }
    private void StopGrounded(){grounded = false;}
}
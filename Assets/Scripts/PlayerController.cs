using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController Controller;
    Vector3 MovementVector;
    Vector3 JMovementVector;
    Vector3 MovementTempHolder;
    Vector3 Movement;
    Vector3 Velocity;

    [Header("PlayerController Settings")]
    public float DefaultMovementSpeed = 8F;
    [HideInInspector] public float MovementSpeed = 0F;
    public float JumpHeight = 1.2F;

    [HideInInspector] float Gravity = -16F;
    [HideInInspector] public float GroundDistance = 1F;
    [HideInInspector] public bool IsJumpPressed = false; 
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public float MovementX = 0;
    [HideInInspector] public float JMovementX = 0;
    [HideInInspector] public float MovementZ = 0;
    [HideInInspector] public float JMovementZ = 0;
    public Transform FeetCollider;
    public LayerMask GroundLayerMask;

    void Start()
        {
            MovementSpeed = DefaultMovementSpeed;
        }

    void Update()
        {

            if(isGrounded && Velocity.y < -2F)//reseting the gravitational velocity on impact with the ground
                {
                    Velocity.y = -1F;
                    IsJumpPressed = false;
                }
            if(!isGrounded && Velocity.y > 0)//reseting the gravitational velocity on impact with the ground
                {
                    IsJumpPressed = true;
                }

            if (Input.GetButtonDown("Jump") && isGrounded) //jump
                {
                    //IsJumpPressed = true;
                    Velocity.y = Mathf.Sqrt(JumpHeight * -2F * Gravity);
                }
                if(isGrounded && !IsJumpPressed)
                    {
                        MovementX = Input.GetAxisRaw("Horizontal");
                        MovementZ = Input.GetAxisRaw("Vertical");
                    }
                
                if(!isGrounded && IsJumpPressed)
                    {
                        JMovementX = Input.GetAxisRaw("Horizontal");
                        JMovementZ = Input.GetAxisRaw("Vertical");
                    }
                else
                    {
                        JMovementX = 0;
                        JMovementZ = 0;
                    }

            //applying input and physics
            MovementVector = (transform.right * MovementX + transform.forward * MovementZ);
            JMovementVector = (transform.right * JMovementX* 0.4F + transform.forward * JMovementZ* 0.4F);

            if(isGrounded && !IsJumpPressed)
                {       
                    Movement = MovementVector.normalized;
                }
            if(!isGrounded && IsJumpPressed)
                {
                    MovementTempHolder =  MovementVector.normalized;
                    Movement = MovementTempHolder + JMovementVector;
                }


            Controller.Move(Movement * MovementSpeed * Time.deltaTime);//moving the player
            //gravity
            Controller.Move(Velocity * Time.deltaTime );
            Velocity.y += Gravity * Time.deltaTime;
        }

    void FixedUpdate()
        {
            isGrounded = Physics.CheckSphere(FeetCollider.position, GroundDistance, GroundLayerMask); //checking if feet touch the ground
        }
}
//if(isGrounded && !IsJumpPressed)
//                {
//                    MovementX = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
//                   MovementZ = Input.GetAxisRaw("Vertical") * ;
//                }
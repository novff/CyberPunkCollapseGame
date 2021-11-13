using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController Controller;
    Vector3 MovementVector;
    Vector3 Velocity;

    [Header("PlayerController Settings")]
    public float DefaultMovementSpeed = 8F;
    public float MovementSpeed = 0F;
    public float JumpHeight = 1.2F;

    [HideInInspector] public float Gravity = -9.8F;
    [HideInInspector] public float GroundDistance = 0.2F;
    [HideInInspector] public bool IsJumpPressed = false; 
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public float MovementX = 0;
    [HideInInspector] public float MovementZ = 0;
    public Transform FeetCollider;
    public LayerMask GroundLayerMask;

    void Start()
        {
            MovementSpeed = DefaultMovementSpeed;
        }

    void Update()
        {
            if(isGrounded && Velocity.y < 0)
                {
                    Velocity.y = -1F;
                    IsJumpPressed = false;
                }

            //getting info on localized movement
            if(isGrounded && !IsJumpPressed)
                {
                    MovementX = Input.GetAxisRaw("Horizontal");
                    MovementZ = Input.GetAxisRaw("Vertical");
                }
            
            if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    IsJumpPressed = true;
                    Velocity.y = Mathf.Sqrt(JumpHeight * -2F * Gravity);
                }

            //applying input and physics
            MovementVector = transform.right * MovementX + transform.forward * MovementZ;
            Velocity.y += Gravity * Time.deltaTime;

            //moving the player
            Controller.Move(MovementVector.normalized * MovementSpeed * Time.deltaTime);
            Controller.Move(Velocity * Time.deltaTime);
            

            //PlayerAnimator.SetFloat("Horizontal", Movement.x);
            //PlayerAnimator.SetFloat("Vertical", Movement.y);
            //PlayerAnimator.SetFloat("Speed", Movement.sqrMagnitude);
        }

    void FixedUpdate()
        {
            isGrounded = Physics.CheckSphere(FeetCollider.position, GroundDistance, GroundLayerMask);
        }
}

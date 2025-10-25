// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.EventSystems;

// public class PlayerMovement : MonoBehaviour
// {


//     [Header("Movement")]
//     public float movementSpeed;

//     private bool grounded;
//     public float groundDrag;

//     public float jumpForce;
//     public float jumpCooldown;
//     public float airMultiplier;


//     //Dash variables
//     public float dashSpeed;       
//     public float dashDuration;
//     public float dashCooldown;
//     private bool canDash = true;
//     bool readyToJump;

//     [Header("Keybindings")]
//     public KeyCode jumpKey = KeyCode.Space;
//     public KeyCode dashKey = KeyCode.LeftShift;

//     [Header("GroundCheck")]
//     public float playerHeight;
//     public LayerMask IsGround;


//     public Transform orientation;

//     public Animator swordAnimator;

//     float horizontalInput;
//     float verticalInput;
//     float yRotation;

//     Vector3 movementDirection;

//     Rigidbody rb;
//     private bool isDashing = false;


//     // Start is called before the first frame update
//     void Start()
//     {
//         if (orientation == null)
//         {
//             orientation = transform; // Assign to player transform if not set
//         }
//         readyToJump = true;
//         rb = GetComponent<Rigidbody>();
//         //rb.freezeRotation = false;
//     }


//     // Update is called once per frame
//     void Update()
//     {
//         grounded = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + 0.2f);
//         KeyBoardInputs();
//         SpeedControl();



//         Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red, 0.1f);
//         Debug.Log("Grounded: " + grounded);

//         if(grounded)
//         {
//             rb.drag = groundDrag;
//         }
//         else
//         {
//             rb.drag = 0;
//         }

//         Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
//         bool isMoving = flatVelocity.magnitude > 0.1f;

//         if(swordAnimator != null)
//         {
//             swordAnimator.enabled = isMoving;
//         }
//     }
//     private void FixedUpdate()
//     {
//         MovePlayer();
//     }

//     private void KeyBoardInputs()
//     {
//         horizontalInput = Input.GetAxisRaw("Horizontal");
//         verticalInput = Input.GetAxisRaw("Vertical");
//         if(Input.GetKey(jumpKey) && readyToJump && grounded)
//         {
//             readyToJump = false;

//             Jump();
//             Invoke(nameof(ResetJump), jumpCooldown);
//         }
//     }

//     private void MovePlayer()
//     {
//         // calculate movement direction

//         movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

//         if(grounded)
//         {
//             rb.AddForce(movementDirection*movementSpeed*10f, ForceMode.Force);
//         }
//         else if(grounded == false)
//         {
//             rb.AddForce(movementDirection*movementSpeed*10f*airMultiplier, ForceMode.Force);
//         }
//     }

//     private void SpeedControl()
//     {
//         Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
//         if(flatVelocity.magnitude >movementSpeed)
//         {
//             Vector3 limitedVel = flatVelocity.normalized*movementSpeed;
//             rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
//         }
//     }

//     private void Jump()
//     {
//         rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
//         rb.AddForce(transform.up*jumpForce, ForceMode.Impulse);
//     }

//     private void ResetJump()
//     {
//         readyToJump = true;
//     }
// }

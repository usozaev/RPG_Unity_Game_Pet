using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    // Dash Variables
    public float dashSpeed;           // Speed multiplier when dashing
    public float dashDuration;        // How long the dash lasts
    public float dashCooldown;        // Cooldown time before you can dash again
    private bool canDash = true;      // To track if you can dash again

    bool readyToJump;

    [Header("Keybindings")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift; // Default dash key

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask IsGround;

    public Transform orientation;

    public Animator swordAnimator;

    float horizontalInput;
    float verticalInput;
    Vector3 movementDirection;

    Rigidbody rb;

    private bool isDashing = false;  // To check if the player is currently dashing

    private bool grounded;  // Declare grounded variable

    void Start()
    {
        if (orientation == null)
        {
            orientation = transform; // Assign to player transform if not set
        }
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGroundStatus();
        KeyBoardInputs();
        SpeedControl();
        AnimateMovement();
    }

    void FixedUpdate()
    {
        if (!isDashing)  // Only move the player if not dashing
        {
            MovePlayer();
        }
    }

    // Ground check using Raycast or CheckSphere
    private void CheckGroundStatus()
    {
        // Using Physics.Raycast to check if the player is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, IsGround);
    }

    // Keyboard inputs
    private void KeyBoardInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump input
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Dash input
        if (Input.GetKeyDown(dashKey) && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    // Move the player
    private void MovePlayer()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float currentSpeed = grounded ? movementSpeed : movementSpeed * airMultiplier;

        rb.AddForce(movementDirection * currentSpeed * 10f, ForceMode.Force);
    }

    // Control player speed
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVelocity.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // Jumping logic
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Prevent horizontal speed loss
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // Reset jump status after cooldown
    private void ResetJump()
    {
        readyToJump = true;
    }

    // Dash Coroutine
    private IEnumerator Dash()
    {
        isDashing = true;

        // Temporarily increase speed
        float originalSpeed = movementSpeed;
        movementSpeed *= dashSpeed;  // Apply dash speed multiplier

        // Record initial dash direction
        Vector3 dashDirection = movementDirection.normalized;
        
        // Dash Duration
        float dashTime = 0f;
        while (dashTime < dashDuration)
        {
            // Apply force instantly in dash direction
            rb.velocity = new Vector3(dashDirection.x * movementSpeed, rb.velocity.y, dashDirection.z * movementSpeed);

            dashTime += Time.deltaTime;
            yield return null;  // Wait until the next frame
        }

        // Reset speed back after dash is finished
        movementSpeed = originalSpeed;

        // Dash is complete, resetting
        isDashing = false;

        // Cooldown before next dash is allowed
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // Control animation state based on movement
    private void AnimateMovement()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        bool isMoving = flatVelocity.magnitude > 0.1f;

        if (swordAnimator != null)
        {
            swordAnimator.SetBool("IsMoving", isMoving); // Update animator's movement state
        }

        // Adjust drag when grounded
        rb.drag = grounded ? groundDrag : 0;
    }
}

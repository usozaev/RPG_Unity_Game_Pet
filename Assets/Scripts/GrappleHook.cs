using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [Header("Grapple Settings")]
    public float grappleDistance = 20f;  // Maximum grapple distance
    public float grappleSpeed = 10f;     // Speed at which the player is pulled towards the grapple point
    public float grappleCooldown = 1f;   // Cooldown time before you can grapple again
    private bool canGrapple = true;      // To track if you can grapple again

    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    private bool isGrappling = false;    // Is the player currently grappling
    private Rigidbody rb;
    private Transform player;
    private float originalDrag;

    [Header("Keybindings")]
    public KeyCode grappleKey = KeyCode.G;  // Key to fire the grapple

    [Header("GroundCheck")]
    public LayerMask grappleLayer;  // Only surfaces that can be grappled

    public GameObject lineRendererObject;  // Reference to your LineRenderer GameObject
    public Camera playerCamera;  // Reference to the player's camera (if needed)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = transform;
        lineRenderer = lineRendererObject.GetComponent<LineRenderer>();  // Get the LineRenderer component from the assigned object
        playerCamera = Camera.main;  // Use the main camera

        // Set LineRenderer to world space
        lineRenderer.useWorldSpace = true;

        // Set Start and End Width programmatically
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Disable LineRenderer initially
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(grappleKey) && canGrapple && !isGrappling)
        {
            ShootGrapple();
        }

        if (isGrappling)
        {
            PullTowardsGrapple();
        }
    }

    void ShootGrapple()
    {
        RaycastHit hit;

        // Cast a ray based on the camera's forward direction, not just the player's forward direction
        Vector3 shootDirection = playerCamera.transform.forward;  // Get the direction the camera is facing
        Debug.DrawRay(player.position, shootDirection * grappleDistance, Color.green, 1f);  // Visualize the ray in the scene view

        // Cast a ray in front of the camera to detect a grapple point
        if (Physics.Raycast(player.position, shootDirection, out hit, grappleDistance, grappleLayer))
        {
            // If the ray hits an object, set the grapple point
            grapplePoint = hit.point;
            isGrappling = true;

            // Enable the line renderer to show the grapple
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, player.position);  // Start point (player's position)
            lineRenderer.SetPosition(1, grapplePoint);  // End point (grapple point)

            // Disable gravity while grappling to stop the player from falling
            originalDrag = rb.drag;
            rb.drag = 10f;

            // Disable the ability to grapple again until cooldown is complete
            canGrapple = false;
            Invoke(nameof(ResetGrapple), grappleCooldown);  // Call ResetGrapple after cooldown
        }
        else
        {
            Debug.Log("Grapple ray did not hit anything");
        }
    }

    void PullTowardsGrapple()
    {
        // If the player is not at the grapple point yet
        if (Vector3.Distance(player.position, grapplePoint) > 2f)
        {
            // Calculate direction towards the grapple point and apply a force to pull the player
            Vector3 direction = (grapplePoint - player.position).normalized;
            rb.AddForce(direction * grappleSpeed, ForceMode.Force);

            // Update the line renderer start and end points during movement
            lineRenderer.SetPosition(0, player.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
        else
        {
            // The player has reached the grapple point, so stop the grapple
            isGrappling = false;
            lineRenderer.enabled = false;  // Disable the line when done

            // Reset the drag and stop the player's movement
            rb.drag = originalDrag;  // Restore original drag
            rb.velocity = Vector3.zero;  // Stop the player's movement
        }
    }

    void ResetGrapple()
    {
        canGrapple = true;
        Debug.Log("Grapple ready!");
    }
}

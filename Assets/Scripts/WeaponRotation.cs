using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public Transform cameraTransform; // Assign Main Camera
    public Transform swordHand;       // Assign the SwordHand
    public Vector3 swordLocalOffset = new Vector3(0.2f, -0.2f, 0.5f); // Initial offset for sword positioning
    public float smoothSpeed = 10f; // Rotation smoothness
    public float distanceAdjustmentFactor = 0.1f; // Adjust sword distance based on camera pitch

    private Transform sword;
    public Animator swordAnimator; // Assign sword's Animator in inspector

    void Start()
    {
        if (swordHand == null)
        {
            Debug.LogError("WeaponRotation: SwordHand is not assigned!");
            return;
        }

        if (swordHand.childCount > 0)
        {
            sword = swordHand.GetChild(0); // Automatically find the sword if it's a child of SwordHand
        }

        if (swordAnimator != null)
        {
        swordAnimator.enabled = false; // Disable idle animation initially
        }
    }

    void Update()
    {
        if (cameraTransform == null || swordHand == null || sword == null)
        {
            Debug.LogError("WeaponRotation: Missing references!");
            return;
        }

        // Get the camera's pitch (X rotation)
        float cameraPitch = cameraTransform.eulerAngles.x;

        // Normalize the pitch to prevent flipping when looking directly up or down
        if (cameraPitch > 180f) cameraPitch -= 360f;

        // Rotate the SwordHand based on the camera's vertical rotation (up/down)
        Quaternion targetRotation = Quaternion.Euler(cameraPitch, swordHand.eulerAngles.y, swordHand.eulerAngles.z);
        swordHand.rotation = Quaternion.Slerp(swordHand.rotation, targetRotation, Time.deltaTime * smoothSpeed);

        // Calculate adjusted sword offset based on camera pitch (up/down)
        Vector3 adjustedOffset = swordLocalOffset;

        // Adjust sword's Z-position based on camera pitch
        adjustedOffset.z = Mathf.Lerp(swordLocalOffset.z, swordLocalOffset.z - distanceAdjustmentFactor * Mathf.Sign(cameraPitch), Time.deltaTime * smoothSpeed);

        // Keep the sword at the adjusted position relative to sword hand
        sword.localPosition = adjustedOffset;

        // Keep sword aligned with the camera's forward direction
        sword.forward = Vector3.Slerp(sword.forward, cameraTransform.forward, Time.deltaTime * smoothSpeed);

    }
    
    
}
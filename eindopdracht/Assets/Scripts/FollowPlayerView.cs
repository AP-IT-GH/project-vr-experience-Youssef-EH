using UnityEngine;

public class FollowPlayerView : MonoBehaviour
{
    public Transform targetCamera;   // XR Rig camera
    public float distance = 1.0f;    // Distance in front of the camera
    public float yOffset = 0.0f;     // Extra height offset if needed

    void Update()
    {
        if (targetCamera == null) return;

        // Project camera forward onto XZ plane (ignore head tilt)
        Vector3 forward = targetCamera.forward;
        forward.y = 0;
        forward.Normalize();

        // Position in front of camera at eye height (+ optional offset)
        Vector3 targetPos = targetCamera.position + forward * distance;
        targetPos.y = targetCamera.position.y + yOffset;
        transform.position = targetPos;

        // Only rotate around Y (yaw)
        Vector3 lookPos = targetCamera.position;
        lookPos.y = transform.position.y; // keep upright
        transform.LookAt(lookPos);
        transform.Rotate(0, 180, 0); // Face the player, not away
    }
}
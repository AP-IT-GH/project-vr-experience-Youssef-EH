using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform playerCamera; // XR Rig Main Camera
    public float distance = 1.0f; // Distance in front of the camera
    public float yOffset = 0.0f; // Extra height offset if needed

    void Update()
    {
        if (!playerCamera) return;

        // Project camera forward onto XZ plane (ignore head tilt)
        var forward = playerCamera.forward;
        forward.y = 0;
        forward.Normalize();

        // Position in front of camera at eye height (+ optional offset)
        var targetPos = playerCamera.position + forward * distance;
        targetPos.y = playerCamera.position.y + yOffset;
        transform.position = targetPos;

        // Only rotate around Y (yaw)
        var lookPos = playerCamera.position;
        lookPos.y = transform.position.y; // keep upright
        transform.LookAt(lookPos);
        transform.Rotate(0, 180, 0); // Face the player, not away
    }
}
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform playerCamera;
    private Vector3 _lookAtPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCamera)
        {
            // Setting the lookAtPosition to the player's position but keeping the y-coordinate of the object
            _lookAtPosition = new Vector3(playerCamera.position.x, transform.position.y, playerCamera.position.z);
            // Setting the rotation of the object to look at the player's position
            transform.LookAt(_lookAtPosition);
            // Inverting the rotation to face the player
            transform.forward = -transform.forward;
        }
    }
}

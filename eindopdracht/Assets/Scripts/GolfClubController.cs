using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GolfClubController : MonoBehaviour
{
    [SerializeField] private float minForceThreshold = 0.5f;
    [SerializeField] private float forceMultiplier = 15f;
    
    private Rigidbody clubRigidbody;
    private Vector3 previousPosition;
    private Vector3 velocity;
    
    private void Start()
    {
        clubRigidbody = GetComponent<Rigidbody>();
        previousPosition = transform.position;
    }
    
    private void FixedUpdate()
    {
        // Calculate club velocity
        velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        previousPosition = transform.position;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Check if we're hitting a golf ball
        if (collision.gameObject.CompareTag("GolfBall"))
        {
            // Calculate hit force based on velocity
            float hitMagnitude = velocity.magnitude;
            
            // Only apply force if swing is hard enough
            if (hitMagnitude > minForceThreshold)
            {
                // Get direction from club to ball
                Vector3 hitDirection = (collision.contacts[0].point - clubRigidbody.position).normalized;
                
                // Apply force to the ball
                Rigidbody ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                ballRigidbody.AddForce(hitDirection * hitMagnitude * forceMultiplier, ForceMode.Impulse);
                
                // Optionally add some audio feedback here
            }
        }
    }
}
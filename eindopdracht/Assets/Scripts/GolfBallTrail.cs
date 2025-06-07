using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class GolfBallTrail : MonoBehaviour
{
    public float maxTrailLength = 3f;        // Maximum length of the trail
    public float minSpeedForTrail = 0.5f;    // Speed below which the trail disappears

    private LineRenderer lr;
    private Rigidbody rb;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.startWidth = 0.04f;
        lr.endWidth = 0.01f;
        lr.enabled = false;

        // Set up a white-to-transparent gradient in script
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f), // opaque at start
                new GradientAlphaKey(0.0f, 1.0f)  // transparent at end
            }
        );
        lr.colorGradient = gradient;
    }

    void Update()
    {
        float speed = rb.linearVelocity.magnitude;

        if (speed > minSpeedForTrail)
        {
            lr.enabled = true;
            Vector3 velocityDir = rb.linearVelocity.normalized;
            float trailLength = Mathf.Min(maxTrailLength, speed * 0.4f); // length scales with speed

            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position - velocityDir * trailLength);
        }
        else
        {
            lr.enabled = false;
        }
    }
}
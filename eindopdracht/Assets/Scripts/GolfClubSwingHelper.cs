using UnityEngine;
using UnityEngine.UI;

public class GolfClubSwingHelper : MonoBehaviour
{
    [Header("References")]
    public Transform swingPoint;          // Assign to club's hitting tip
    public LineRenderer aimLine;          // Assign a LineRenderer (thin, material set, world space ON)
    public Image powerBarImage;           // Assign a UI Image (Type: Filled, Fill Method: Horizontal)
    public Image zoneIndicator;           // Optional: assign a UI image for "in zone" indicator

    [Header("Swing Settings")]
    public KeyCode swingKey = KeyCode.F;
    public KeyCode cancelKey = KeyCode.Space;
    public float minForce = 1f;           // Lower tap force
    public float maxForce = 25f;
    public float holdTimeToMax = 2f;      // How long (seconds) to reach full power
    public float hitZoneRadius = 1.2f;    // Distance from club to ball to allow hitting

    [Header("Ball Respawn")]
    public float respawnYThreshold = -2f;

    private float holdTimer = 0f;
    private bool isCharging = false;
    private Rigidbody lastBall;
    private Vector3 lastBallValidPos;
    private Rigidbody golfBall;
    private bool isInZone = false;

    void Start()
    {
        if (aimLine != null)
        {
            aimLine.positionCount = 2;
            aimLine.startWidth = 0.02f;
            aimLine.endWidth = 0.02f;
            aimLine.useWorldSpace = true;
            aimLine.enabled = true; // Always show direction line when in zone
            aimLine.startColor = Color.white;
            aimLine.endColor = Color.white;
        }
        if (powerBarImage != null)
        {
            powerBarImage.fillAmount = 0;
            powerBarImage.color = Color.green;
        }
        if (zoneIndicator != null)
        {
            zoneIndicator.enabled = false;
        }
    }

    void Update()
    {
        golfBall = GetClosestBall();

        // Ball respawn
        if (golfBall != null)
        {
            if (golfBall.position.y > respawnYThreshold)
            {
                lastBall = golfBall;
                lastBallValidPos = golfBall.position;
            }
            else
            {
                RespawnBall(golfBall);
            }
        }

        // Check if in hit zone
        isInZone = (golfBall != null && swingPoint != null && 
                    Vector3.Distance(swingPoint.position, golfBall.position) <= hitZoneRadius);

        // Zone indicator logic
        if (zoneIndicator != null)
            zoneIndicator.enabled = isInZone;

        // Always show a thin white line if in zone, not charging
        if (aimLine != null)
        {
            if (isInZone && !isCharging)
            {
                aimLine.enabled = true;
                aimLine.startWidth = 0.02f;
                aimLine.endWidth = 0.02f;
                aimLine.SetPosition(0, swingPoint.position);
                aimLine.SetPosition(1, golfBall.position);
                aimLine.startColor = Color.white;
                aimLine.endColor = Color.white;
            }
            else if (!isInZone)
            {
                aimLine.enabled = false;
            }
        }

        // Power charging logic
        if (isInZone && Input.GetKeyDown(swingKey))
        {
            isCharging = true;
            holdTimer = 0f;
        }
        if (isCharging)
        {
            // Cancel logic
            if (Input.GetKeyDown(cancelKey))
            {
                isCharging = false;
                UpdatePowerBar(0f);
                if (aimLine != null) // revert line
                {
                    aimLine.startColor = Color.white;
                    aimLine.endColor = Color.white;
                    aimLine.startWidth = 0.02f;
                    aimLine.endWidth = 0.02f;
                }
                return;
            }

            if (Input.GetKey(swingKey))
            {
                holdTimer += Time.deltaTime;
                float charge = Mathf.Clamp01(holdTimer / holdTimeToMax);
                UpdatePowerBar(charge);

                // Show colored/thicker aim line while charging
                if (aimLine != null)
                {
                    aimLine.enabled = true;
                    aimLine.SetPosition(0, swingPoint.position);
                    aimLine.SetPosition(1, golfBall.position);
                    Color c = Color.Lerp(Color.green, Color.red, charge);
                    aimLine.startColor = c;
                    aimLine.endColor = c;
                    aimLine.startWidth = 0.05f;
                    aimLine.endWidth = 0.05f;
                }
            }
            if (Input.GetKeyUp(swingKey))
            {
                float charge = Mathf.Clamp01(holdTimer / holdTimeToMax);
                float force = Mathf.Lerp(minForce, maxForce, charge);
                TryHitBall(golfBall, force);
                isCharging = false;
                UpdatePowerBar(0f);
                // Revert aim line to white/thin
                if (aimLine != null)
                {
                    aimLine.startColor = Color.white;
                    aimLine.endColor = Color.white;
                    aimLine.startWidth = 0.02f;
                    aimLine.endWidth = 0.02f;
                }
            }
        }
    }

    Rigidbody GetClosestBall()
    {
        Rigidbody closest = null;
        float minDist = Mathf.Infinity;
        foreach (var go in GameObject.FindGameObjectsWithTag("GolfBall"))
        {
            var rb = go.GetComponent<Rigidbody>();
            if (rb == null) continue;
            float dist = Vector3.Distance(swingPoint.position, rb.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = rb;
            }
        }
        return closest;
    }

    void TryHitBall(Rigidbody golfBall, float force)
    {
        if (swingPoint == null || golfBall == null) return;
        float dist = Vector3.Distance(swingPoint.position, golfBall.position);
        if (dist < hitZoneRadius)
        {
            Vector3 direction = (golfBall.position - swingPoint.position).normalized;
            golfBall.isKinematic = false; // Ensure physics works
            golfBall.AddForce(direction * force, ForceMode.Impulse);
        }
    }

    void UpdatePowerBar(float charge)
    {
        if (powerBarImage == null) return;
        powerBarImage.fillAmount = charge;
        powerBarImage.color = Color.Lerp(Color.green, Color.red, charge);
    }

    void RespawnBall(Rigidbody golfBall)
    {
        if (lastBall != null)
        {
            golfBall.linearVelocity = Vector3.zero;
            golfBall.angularVelocity = Vector3.zero;
            golfBall.position = lastBallValidPos + Vector3.up * 0.2f;
        }
    }
}
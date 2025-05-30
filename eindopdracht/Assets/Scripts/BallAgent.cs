using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.UIElements;

public class BallAgent : Agent
{
    public Transform holeTransform;
    public Transform aim;
    public Rigidbody rb;

    public float rotationSpeed = 50f;
    public float maxForce = 5f;
    public float minSpeed = 0.05f;

    private bool canShoot = true;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private float previousDistance;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        canShoot = true;
        previousDistance = Vector3.Distance(transform.position, holeTransform.position);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(new Vector2(transform.forward.x, transform.forward.z));
        sensor.AddObservation(rb.linearVelocity); //snelheid bal
        sensor.AddObservation(canShoot ? 1f : 0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (transform.position.y < -5f)
        {
            EndEpisode();
        }
        if (rb.linearVelocity.magnitude > minSpeed)
            return;

        float rotationInput = actions.ContinuousActions[0];
        float shootInput = actions.ContinuousActions[1];

        aim.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
        Vector3 currentEuler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, aim.eulerAngles.y, 0f);

        float currentDistance = Vector3.Distance(transform.position, holeTransform.position);
        float distanceDelta = previousDistance - currentDistance;

        //AddReward(distanceDelta * 0.1f);
        //print(distanceDelta * 0.1f);
        previousDistance = currentDistance;


        if (canShoot)
        {
            float forceToApply = Mathf.Lerp(0f, maxForce, Mathf.Abs(shootInput));

            Vector3 shootDirection = new Vector3(aim.forward.x, 0f, aim.forward.z).normalized;
            rb.AddForce(shootDirection * forceToApply, ForceMode.Impulse);
            canShoot = false;
            AddReward(-0.01f);
        }

        if (rb.linearVelocity.magnitude < minSpeed)
        {
            canShoot = true;
        }

        float distance = Vector3.Distance(transform.position, holeTransform.position);
        if (distance < 0.1f)
        {
            SetReward(1f); 
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuous = actionsOut.ContinuousActions;
        continuous[0] = Input.GetAxis("Horizontal"); 
        continuous[1] = Input.GetKey(KeyCode.Space) ? 1f : 0f; 
    }

    public void LateUpdate()
    {
        Vector3 flatForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        if (flatForward != Vector3.zero)
        {
            aim.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
        }
    }
}

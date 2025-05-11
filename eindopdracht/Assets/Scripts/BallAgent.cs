using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class BallAgent : Agent
{
    public Transform holeTransform;
    public Transform aim;
    public Rigidbody rb;

    public float rotationSpeed = 100f;
    public float maxForce = 5f;
    public float minSpeed = 0.05f;

    private bool canShoot = true;
    private Vector3 startPosition;
    private Quaternion startRotation;

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
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.forward); //positie bal
        sensor.AddObservation(holeTransform.position); //positie hole
        sensor.AddObservation(rb.linearVelocity.magnitude); //snelheid
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (transform.position.y < -5f)
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        if (rb.linearVelocity.magnitude > minSpeed)
            return; 

        float rotationInput = actions.ContinuousActions[0];
        float shootInput = actions.ContinuousActions[1];

        aim.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
        Vector3 currentEuler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, aim.eulerAngles.y, 0f);

        if (shootInput > 0.5f && canShoot)
        {
            Vector3 shootDirection = new Vector3(aim.forward.x, 0f, aim.forward.z).normalized;
            rb.AddForce(shootDirection * maxForce, ForceMode.Impulse);
            AddReward(-0.01f); 
            canShoot = false;
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

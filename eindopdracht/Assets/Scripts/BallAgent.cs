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

    public float rotationSpeed = 70f;
    public float maxForce = 8f;
    public float minSpeed = 0.05f;

    private bool canShoot = true;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private float previousDistance;

    int shotsTaken = 0;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public override void OnEpisodeBegin()
    {
        shotsTaken = 0;

        transform.position = startPosition;
        aim.rotation = startRotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        canShoot = true;
        previousDistance = Vector3.Distance(transform.position, holeTransform.position);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 toHole = holeTransform.position - transform.position;
        Vector3 relativeToHole = aim.InverseTransformDirection(toHole.normalized);
        float distanceToHole = Vector3.Distance(transform.position, holeTransform.position);

        sensor.AddObservation(distanceToHole);
        sensor.AddObservation(relativeToHole);
        sensor.AddObservation(rb.linearVelocity);
        sensor.AddObservation(canShoot ? 1f : 0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 toHole = (holeTransform.position - transform.position).normalized;
        float alignment = Vector3.Dot(aim.forward, toHole);

        AddReward(-0.001f);

        if (transform.position.y < -5f)
        {
            SetReward(-1f);
            EndEpisode();
        }
        if(shotsTaken>100)
        {
            SetReward(-1f);
            EndEpisode();
        }

        if (rb.linearVelocity.magnitude > minSpeed)
            return;

        float rotationInput = actions.ContinuousActions[0];
        float shootInput = actions.ContinuousActions[1];

        aim.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, aim.eulerAngles.y, 0f);

        float currentDistance = Vector3.Distance(transform.position, holeTransform.position);
        float distanceDelta = previousDistance - currentDistance;
        //if (alignment > 0.9f)
        //    AddReward(0.01f);
        previousDistance = currentDistance;

        if (canShoot && shootInput>0.2f)
        {
            float forceToApply = Mathf.Lerp(0f, maxForce, Mathf.Abs(shootInput));

            //if (alignment < 0.5f)
            //    AddReward(-0.05f);

            rb.AddForce(aim.forward.normalized * forceToApply, ForceMode.Impulse);
            canShoot = false;
            AddReward(-0.01f);

            shotsTaken++;
            print(shotsTaken);
        }

        if (rb.linearVelocity.magnitude < minSpeed)
        {
            canShoot = true;
        }

        if (currentDistance < 0.1f)
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
        aim.position = transform.position;

        Vector3 flatForward = new Vector3(aim.forward.x, 0f, aim.forward.z).normalized;
        if (flatForward != Vector3.zero)
        {
            aim.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
        }
    }
}

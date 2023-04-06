using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Ball : Agent
{
    [SerializeField] Transform target;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floor;
    public override void OnEpisodeBegin()
    {
        //transform.position = Vector3.zero;
        transform.localPosition = new Vector3(Random.Range(-5f, 0f), 0, Random.Range(-2f, 2f));
        target.localPosition = new Vector3(Random.Range(1.2f, 2.2f), 0.5f, Random.Range(-2f, 2f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        /*        sensor.AddObservation(transform.position);
                sensor.AddObservation(target.position);*/
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        float speed = 5f;
        //transform.position += new Vector3(moveX, 0, moveY) * Time.deltaTime * speed;
        transform.localPosition += new Vector3(moveX, 0, moveY) * Time.deltaTime * speed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1f);
            floor.material = winMaterial;
            EndEpisode();
        }

        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            floor.material = loseMaterial;
            EndEpisode();
        }
    }
}

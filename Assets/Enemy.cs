using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float turnSpeed = 10;

    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    private void Start()
    {
        waypoints = FindFirstObjectByType<WaypointManager>().GetWaypoints();
    }



    private void Update()
    {
        FaceTarget(agent.steeringTarget);

        // Check if the agent is close to current target point
        if (agent.remainingDistance < .5f)
        {
            // Set the destination to the next waypoint
            agent.SetDestination(GetNextWaypoint());
        }
    }

    private void FaceTarget(Vector3 newTarget)
    {
        // Calculate the direction from current position to the new target
        Vector3 directionToTarget = newTarget - transform.position;
        directionToTarget.y = 0; // Ignore any diffrence in the vertical position // Removes vertical component

        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    private Vector3 GetNextWaypoint()
    {
        if (waypointIndex >= waypoints.Length)
        {
            //waypointIndex = 0;
            return transform.position;
        }
        Vector3 targetPoint = waypoints[waypointIndex].position;
        waypointIndex = waypointIndex + 1;

        return targetPoint;
    }
}

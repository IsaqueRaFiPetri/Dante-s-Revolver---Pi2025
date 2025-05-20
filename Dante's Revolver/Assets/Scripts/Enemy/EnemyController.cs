using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviourPunCallbacks
{
    [SerializeField]private Transform target;
    [SerializeField]private NavMeshAgent agent;

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<IPlayable>() != null)
        {
            target = collider.transform;
        }
    }
    public void MoveToTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
            agent.isStopped = false;
        }
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 incomingVector = hit.point - transform.position;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        Quaternion rotation = (agent.desiredVelocity).normalized != Vector3.zero ? Quaternion.LookRotation((agent.desiredVelocity).normalized) : transform.rotation;
        transform.rotation = rotation;
    }
}
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IKillable
{
    [PunRPC]
    public void TakeDamage(int damage);
}
public interface ISeekable
{
    public void OnTargetLocked();
    public void OntargetAlmostMissing();
    public void OnTargetMiss();
}

public class EnemyController : MonoBehaviourPunCallbacks, IKillable, ISeekable
{
    [SerializeField]private Transform target;
    [SerializeField]private NavMeshAgent agent;
    [SerializeField]Stats enemyStats;

    float lifeValue;

    private void Start()
    {
        agent.speed = enemyStats.moveSpeed;
        lifeValue = enemyStats.lifeValue;
    }

    protected void MoveToTarget()
    {
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

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.GetComponent<IPlayable>() != null)
        {
            print("entered");
            target = collision.transform;
        }
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        lifeValue -= damage;
        print(lifeValue);

        if(lifeValue <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTargetLocked()
    {
        agent.SetDestination(target.transform.position);
        agent.isStopped = false;
    }

    public void OntargetAlmostMissing()
    {
        throw new System.NotImplementedException();
    }

    public void OnTargetMiss()
    {
        throw new System.NotImplementedException();
    }
}
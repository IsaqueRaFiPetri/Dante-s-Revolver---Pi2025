using Photon.Pun;
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
    [SerializeField] Stats enemyStats;

    public enum EnemyState
    {
        Patrolling, Seeking, Running
    }
    public EnemyState enemyState;
    private void Start()
    {
        agent.speed = enemyStats.moveSpeed;
    }
    protected void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<IPlayable>() != null)
        {
            target = collider.transform;
        }
    }

    protected void MoveToTarget()
    {
        if (target != null)
        {
            OnTargetLocked();
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
    [PunRPC]
    public void TakeDamage(int damage)
    {
        enemyStats.lifeValue -= damage;
        print(enemyStats.lifeValue);
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
using UnityEngine;
public class BasicEnemy : EnemyController, ISeeker
{
    public void OnTargetDetect()
    {
        enemyAgent.SetDestination(GetTarget().position);
    }

    public void OnTargetMiss()
    {
        enemyAgent.isStopped = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<IPlayable>() != null)
        {
            SetTarget(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IPlayable>() != null)
        {
            MissTarget();
            OnTargetMiss();
        }
    }
}

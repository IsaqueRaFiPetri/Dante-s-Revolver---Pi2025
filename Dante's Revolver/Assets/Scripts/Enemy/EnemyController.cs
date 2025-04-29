using UnityEngine;
using UnityEngine.AI;

public interface ISeeker
{
    public void OnTargetDetect();
    public void OnTargetMiss();
}
public class EnemyController : MonoBehaviour
{
    [SerializeField]protected NavMeshAgent enemyAgent;
    Transform playerTarget;

    protected Transform SetTarget(Transform target)
    {
        return playerTarget = target;
    }
    protected Transform MissTarget()
    {
        return playerTarget = null;
    }
    protected Transform GetTarget()
    {
        return playerTarget;
    }
}

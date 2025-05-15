using UnityEngine;
using UnityEngine.AI;

public interface ISeeker
{
    public void OnTargetDetect();
    public void OnTargetMiss();
}
public class EnemyController : MonoBehaviour, ISeeker
{
    [SerializeField]protected NavMeshAgent enemyAgent;
    Transform eyePoints;
    Transform playerTarget;
    Ray ray;

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
    public void OnTargetDetect()
    {
        throw new System.NotImplementedException();
    }

    public void OnTargetMiss()
    {
        throw new System.NotImplementedException();
    }
    private void Start()
    {
        ray = new Ray(eyePoints.transform.position, transform.forward);
        OnTargetDetect();
    }
}

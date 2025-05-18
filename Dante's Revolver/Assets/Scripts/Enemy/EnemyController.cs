using UnityEngine;
using UnityEngine.AI;

public interface ISeeker
{
    public void OnTargetDetect();
    public void OnTargetMiss();
    public void DetectCollision();
}
public class EnemyController : MonoBehaviour, ISeeker
{
    [SerializeField]protected NavMeshAgent enemyAgent;
    Transform eyePoints;
    Transform playerTarget;
    Ray ray;
    RaycastHit hit;
    [SerializeField] float maxDistance;

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

    public void DetectCollision()
    {
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<IPlayable>() != null)
            {
                SetTarget(hit.collider.transform);
            }
        }
    }
    private void FixedUpdate()
    {
        DetectCollision();
    }
}

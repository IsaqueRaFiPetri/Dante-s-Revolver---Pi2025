using UnityEngine;
using Photon.Pun;

public interface ILifeable
{

}
public class WeakPoint : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    [SerializeField] ParticleSystem weakPointParticle;
    public GameObject HeadshotParticle()
    {
        return weakPointParticle.gameObject;
    }
    EnemyController GetEnemyController()
    {
        return gameObject.GetComponentInParent<EnemyController>();
    }

    public void TakeDamage(int damage)
    {
        print("enemy headshot -==-=-=-=-=-=-=--=-=: " + damage);
        GetEnemyController().TakeDamage(damage += 100);
    }

    public GameObject GetGameObject()
    {
        return GetEnemyController().gameObject;
    }

}

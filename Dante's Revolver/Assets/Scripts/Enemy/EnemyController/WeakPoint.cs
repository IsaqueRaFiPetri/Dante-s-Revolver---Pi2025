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
        GetEnemyController().GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage * 20000);
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

}

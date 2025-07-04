using System.Collections;
using UnityEngine;
using Photon.Pun;

public interface IWeakeable
{
    public GameObject HeadshotParticle();
}
public class WeakPoint : MonoBehaviourPunCallbacks, IWeakeable
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
    public void Hitkill()
    {
        GetEnemyController().GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 20000);
    }
}

using Photon.Pun;
using UnityEngine;
public interface IDamaging
{
    public void DoDamage(GameObject target);
}
public class DamageInteraction : MonoBehaviourPunCallbacks, IDamaging
{
    [SerializeField] protected WeaponStats weaponsStats;
    [SerializeField] protected GameObject shootParticle;
    [SerializeField] protected ParticleSystem damageParticle;
    [SerializeField] protected ParticleSystem bloodParticle;

    public void DoDamage(GameObject target)
    {
        print(target.name);
        target.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered , weaponsStats.weaponDamage);
    }
    public void ShootParticle(GameObject shootParticle, RaycastHit raycast)
    {
        PhotonNetwork.Instantiate(shootParticle.name, raycast.point + new Vector3(raycast.normal.x * 0.01f, raycast.normal.y * 0.01f, raycast.normal.z * 0.01f), Quaternion.LookRotation(raycast.normal));
    }
}

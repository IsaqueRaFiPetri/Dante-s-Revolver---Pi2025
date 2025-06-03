using Photon.Pun;
using UnityEngine;
public interface IDamaging
{
    public void DoDamage(GameObject target);
}
public class DamageInteraction : MonoBehaviour, IDamaging
{
    //[SerializeField] protected WeaponStats weaponsStats;

    public void DoDamage(GameObject target)
    {
        target.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered /*, weaponsStats.weaponDamage*/);
    }
}

using UnityEngine;
using Photon.Pun;

public class Fireball : MonoBehaviourPunCallbacks, IDamaging
{
    [SerializeField] WeaponStats weaponsStats;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] int damage;

    public void DoDamage(IKillable target)
    {
        target.GetGameObject().GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, weaponsStats.weaponDamage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.GetContact(0).point.normalized);
        PhotonNetwork.Instantiate(explosionParticle.name, transform.position, Quaternion.identity);
        if (collision.collider.TryGetComponent(out PlayerController target))
        {
            DoDamage(target);
        }
        Destroy(gameObject);
    }
}

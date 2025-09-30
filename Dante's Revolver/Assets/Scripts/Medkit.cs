using Photon.Pun;
using UnityEngine;

public class Medkit : Items
{
    IRegenerable _player;
    [SerializeField] float _regenlife;
    public override void Collect()
    {
        PhotonNetwork.Destroy(gameObject);
        _player.RegenLife(_regenlife, true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collect();
    }
}

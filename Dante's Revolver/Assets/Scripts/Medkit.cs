using Photon.Pun;
using UnityEngine;

public class Medkit : Items
{
    IRegenerable _player;
    [SerializeField] float _regenlife;
    [SerializeField] bool _isHealthRegen;
    public override void Collect()
    {
        _player.RegenLife(_regenlife, _isHealthRegen);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IRegenerable _regenerable))
        {
            _player = _regenerable;
            Collect();
        }
    }
}

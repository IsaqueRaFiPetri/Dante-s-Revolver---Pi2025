using UnityEngine;

public class Medkit : Items
{
    IRegenerable _player;
    [SerializeField] float _regenlife;
    public override void Collect()
    {
        _player.RegenLife(_regenlife, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collect();
        if(other.TryGetComponent(out IRegenerable player))
        {
            _player = player;
        }
    }
}

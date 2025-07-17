using UnityEngine;

public class Medkit : Items
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IRegenerable player))
        {
            player.RegenLife(15);
            Destroy(gameObject);
        }
    }
}

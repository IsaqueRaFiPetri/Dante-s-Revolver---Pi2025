using Photon.Pun;
using UnityEngine;

public class BlockOfDamage : DamageInteraction
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out IKillable _ikillable))
        {
            DoDamage(_ikillable);
        }
    }
}

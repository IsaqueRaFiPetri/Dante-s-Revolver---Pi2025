using UnityEngine;

public class CerberusFireRay : DamageInteraction
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IKillable _IKillable))
        {
            if(_IKillable.GetGameObject().TryGetComponent(out PlayerController _playerController))
            {
                DoDamage(_IKillable);
            }
        }
    }
}

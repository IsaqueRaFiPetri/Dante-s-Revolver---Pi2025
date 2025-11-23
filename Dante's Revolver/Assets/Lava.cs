using DG.Tweening;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] float _inpulseForce;
    [SerializeField] int _lavaDamage;
    [SerializeField] bool _isTeleportLava;
    [SerializeField] Transform _revivePoint;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Rigidbody _rb))
        {
            if (_isTeleportLava)
            {
                _rb.transform.DOMove(_revivePoint.position, .25f).SetEase(Ease.InCubic);
            }
            else
            {
                _rb.AddForce(transform.up * _inpulseForce, ForceMode.Impulse);
                if(_rb.TryGetComponent(out IKillable _iKillable))
                {
                    _iKillable.TakeDamage(_lavaDamage);
                }
            }
        }
    }
}

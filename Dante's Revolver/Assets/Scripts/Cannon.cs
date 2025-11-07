using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Cannon : MonoBehaviour
{
    [SerializeField] UnityEvent OnCannonShoot;
    [SerializeField] GameObject _cannonBullet;
    [SerializeField] float _cannonForce;
    Rigidbody _bulletRb;
    public void CannonShoot()
    {
        OnCannonShoot.Invoke();
    }
    public void Shoot()
    {
        _bulletRb = PhotonNetwork.Instantiate(_cannonBullet.name, transform.position, transform.rotation).GetComponent<Rigidbody>();
        _bulletRb.AddForce(transform.forward *= _cannonForce, ForceMode.Impulse);
    }
}

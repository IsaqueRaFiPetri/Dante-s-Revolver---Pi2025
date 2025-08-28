using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    [SerializeField] List<PhotonView> _players;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _players.Add(_playerPhantom.gameObject.GetPhotonView());
        }
        if(other.TryGetComponent(out PlayerController _playerObj))
        {
            _players.Add(_playerObj.photonView);
        }
        if (_players.Count >= 2)
        {
            _playerPhantom.isInReviveArea(transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _players.Remove(_playerPhantom.gameObject.GetPhotonView());
        }
        if (other.TryGetComponent(out PlayerController _playerObj))
        {
            _players.Remove(_playerObj.photonView);
        }
    }
}
